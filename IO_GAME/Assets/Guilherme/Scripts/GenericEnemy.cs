using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using DG.Tweening;

public class GenericEnemy : MonoBehaviour
{
    [System.Serializable]
    public struct TrashDrop
    {
        public GameObject prefab;
        public int amount;
    }
    public enum TypeEnemy { contact, shooter }
    public TypeEnemy type;
    public enum State { patrol, chase }
    public State currentState;
    
    protected NavMeshAgent agent;
    protected BoxCollider2D fisCollider;
    protected CircleCollider2D areaCollider;
    protected Rigidbody2D rb;
    protected PlayerMove player;
    
    public float life = 30f;
    public float damage = 10f;
    public float cooldown = 1f;
    
   [Header("Configuração de Drops")]
    [SerializeField] private List<TrashDrop> listTrashDrops;

    protected bool feltPlayer;
    protected bool isAttack;
    protected bool canAttack;
    bool haveAPoint = false;
    Vector2 randomPoint;

    [Header("Efeitos de Dano")]
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private float knockbackForce = 4f;

    protected virtual void Start()
    {
        currentState = State.patrol;
        player = FindObjectOfType<PlayerMove>();
        agent = GetComponent<NavMeshAgent>();
        
        if(agent != null)
        {
            agent.updateUpAxis = false;
            agent.updateRotation = false;
        }
    }

    protected virtual void Update()
    {
        SwitchStates();
    }

    public virtual void TakeDamage(float amount)
    {
        life -= amount;

        SpawnDamageNumber(amount);
        StartCoroutine(FlashRed());
        StartCoroutine(Knockback());

        if (life <= 0) Die();
    }

    protected virtual void Die()
    {
        foreach (TrashDrop drop in listTrashDrops)
        {
            if (drop.prefab != null)
            {
                for (int i = 0; i < drop.amount; i++)
                {
                    Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                    Instantiate(drop.prefab, transform.position + spawnOffset, Quaternion.identity);
                }
            }
        }

        Destroy(gameObject);
    }
    

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            player = tempPlayer;
            feltPlayer = true;
            currentState = State.chase;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            feltPlayer = false;
            currentState = State.patrol;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    protected virtual void Chase()
    {
        if (agent != null && !agent.enabled) agent.enabled = true;
        if (player != null) agent.SetDestination(player.transform.position);
    }

    protected virtual void Patrol()
    {
        if (agent != null && !agent.enabled) agent.enabled = true;
        agent.isStopped = false;

        if (!haveAPoint)
        {
            float randomValueX = Random.Range(-5f, 5f);
            float randomValueY = Random.Range(-5f, 5f);
            randomPoint = new Vector2(transform.position.x - randomValueX, transform.position.y - randomValueY);
            haveAPoint = true;
        }

        agent.SetDestination(randomPoint);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                haveAPoint = false;
            }
        }
    }

    protected virtual void SwitchStates()
    {
        switch (currentState)
        {
            case State.patrol:
                Patrol();
                break;
            case State.chase:
                Chase();
                haveAPoint = false;
                break;
        }
    }

    private void SpawnDamageNumber(float amount)
{
    if (damageNumberPrefab == null) return;

    Vector3 spawnPos = transform.position + new Vector3(
        Random.Range(-0.3f, 0.3f), 0.5f, 0);

    GameObject obj = Instantiate(damageNumberPrefab, spawnPos, Quaternion.identity);
    obj.GetComponent<DamageNumber>().Init(amount);
}

private IEnumerator FlashRed()
{
    if (spriteRenderer == null) yield break;

    spriteRenderer.color = Color.red;
    yield return new WaitForSeconds(flashDuration);
    spriteRenderer.color = Color.white;
}

private IEnumerator Knockback()
{
    if (agent == null || player == null) yield break;

    agent.isStopped = true;
    agent.updatePosition = false;

    Vector3 direction = (transform.position - player.transform.position).normalized;
    Vector3 targetPos = transform.position + direction * 0.6f;

    yield return transform.DOMove(targetPos, 0.08f)
        .SetEase(Ease.OutQuad)
        .WaitForCompletion();

    agent.Warp(transform.position);
    agent.updatePosition = true;
    agent.isStopped = false;
}
}
