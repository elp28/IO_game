using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public BoxCollider2D fisCollider;
    public CircleCollider2D areaCollider;
    public Rigidbody2D rb;
    bool feltPlayer;
    public bool FeltPlayer => feltPlayer;
    public  float life;
    public  float damage;
    public PlayerSwimming player;
    public bool isAttack;
    public bool canAttack;
    public float cooldown;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerSwimming>();
        if (player != null)
        {   
            feltPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerSwimming>();
        if (player != null)
        {
            feltPlayer = false;
            player = null;
        }
    }

    public void PlayerClosed()
    {

    }

    


}
