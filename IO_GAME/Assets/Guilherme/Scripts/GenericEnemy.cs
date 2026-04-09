using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public enum TypeEnemy { contact, shooter}
    public TypeEnemy type;
    public enum State { patrol, chase, caught}
    public State currentState;
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
    public float pullSpeed = 10f; 
    PlayerCollect bagOfPlayer;
    
    protected virtual void Start()
    {
        isFree = true;

        player = FindObjectOfType<PlayerMove>(); 
    }

    protected virtual void Update()
    {
        /*SwitchStates();
        if (!IsFree && player != null)
        {
            currentState = State.caught;
        }*/
        if (!IsFree && player != null)
        {
            if (agent != null && agent.enabled) agent.enabled = false;
                
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, pullSpeed * Time.deltaTime);
        }
        else if(IsFree && player != null)
        {
            if(agent != null && !agent.enabled)
                agent.enabled = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.isTrigger) return;

        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            player = tempPlayer;
            feltPlayer = true;
        }
    }

    
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            feltPlayer = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

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
            if (!isFree)
            {
                bagOfPlayer.PickedTrash();
                Die();
            }
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {

    }

    public virtual void ItsOverForTrash(bool state, PlayerCollect playerBag)
    {
        isFree = state;
        bagOfPlayer = playerBag;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void Chase()
    {

    }

    protected virtual void Patrol()
    {

    }

    protected virtual void Caught()
    {

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
                break;

            case State.caught:
                Caught();
                break;
        }
    }
}