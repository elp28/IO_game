using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SimpleTrashEnemy : GenericEnemy
{
    GenericEnemy genericEnemy;
    [SerializeField] float cooldown;
    bool isAttack;
    bool canAttack;
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       genericEnemy = this;
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(genericEnemy.FeltPlayer && !canAttack)
        {
            //rb.MovePosition(Vector2.MoveTowards(transform.localPosition, new Vector2(player.transform.position.x, player.transform.position.y), speed * Time.deltaTime));
            agent.SetDestination(player.transform.position);
        }

        if(canAttack && !isAttack)
        {
            StartCoroutine(CicleDamage());
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null) 
        canAttack = true;
    }
    void OnCollisionExit2D(Collision2D collision) 
    {
        canAttack = false;
    }
   
    IEnumerator CicleDamage()
    {
        isAttack = true;
        genericEnemy.player.GetComponent<PlayerLife>().TakeDamage(genericEnemy.damage);
        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }
}
