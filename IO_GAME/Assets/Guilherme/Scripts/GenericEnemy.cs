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
    public float pullSpeed = 3;
    

    protected virtual void Start()
    {
        isFree = true;
    }

    protected virtual void Update()
    {
        if (!IsFree && player != null)
        {
            print("aiai");
            if (agent != null && agent.enabled) agent.enabled = false;

            if(player != null)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, pullSpeed * Time.deltaTime);

        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            player = tempPlayer;
            feltPlayer = true;
        }

        
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            feltPlayer = false;
            player = null;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
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

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {

    }

    public virtual void ItsOverForTrash(bool state)
    {
        isFree = state;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}