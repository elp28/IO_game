using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public enum TypeEnemy { contact, shooter}
    public TypeEnemy type;
    protected NavMeshAgent agent;
    protected BoxCollider2D fisCollider;
    protected CircleCollider2D areaCollider;
    protected Rigidbody2D rb;
    protected bool feltPlayer;
    public float life;
    public float damage;
    protected PlayerMove player;
    protected bool isAttack;
    protected bool canAttack;
    public float cooldown;
    [SerializeField] bool isFree;
    public bool IsFree => isFree;
    

    private void Start()
    {
        isFree = true;
    }

    private void Update()
    {
        if (!IsFree)
        {
            agent.stoppingDistance = 0;
            agent.speed = agent.speed * 10;
            agent.SetDestination(player.transform.position);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            player = tempPlayer;
            feltPlayer = true;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            feltPlayer = false;
            player = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            print("eai");
            if (!isFree)
            {
                print ("iai");
                Destroy(gameObject);
            }
        }
    }

    public void ItsOverForTrash()
    {
        isFree = !isFree;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}