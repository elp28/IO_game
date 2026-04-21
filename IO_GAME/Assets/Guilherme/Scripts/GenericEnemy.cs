using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

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

    // NOVA FUNÇÃO: Receber dano do Arpão
    public virtual void TakeDamage(float amount)
    {
        life -= amount;
        if (life <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        foreach (TrashDrop drop in listTrashDrops)
        {
            if (drop.prefab != null)
            {
                // Faz o spawn da quantidade definida para este item
                for (int i = 0; i < drop.amount; i++)
                {
                    // Adicionamos um pequeno "offset" aleatório para os itens não nascerem um dentro do outro
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
}
