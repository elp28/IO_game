using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
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
    
    [Header("Drop de Lixo")]
    public GameObject trashDropPrefab; // Coloque o prefab do lixo aqui no Inspector

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
        // Dropa o recurso no chão ao morrer
        if (trashDropPrefab != null)
        {
            Instantiate(trashDropPrefab, transform.position, Quaternion.identity);
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