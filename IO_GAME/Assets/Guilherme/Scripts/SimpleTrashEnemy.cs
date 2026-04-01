using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleTrashEnemy : GenericEnemy
{
    void Start()
    {
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    void Update()
    {
        if (IsFree) { return; }
    

        if (feltPlayer && player != null)
        {
            agent.SetDestination(player.transform.position);
        }

        if (canAttack && !isAttack && !IsFree)
        {
            StartCoroutine(CicleDamage());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsFree) { return; }
        agent.isStopped = true;
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
            canAttack = true;

        
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsFree) { return; }
        agent.isStopped = false;
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
        canAttack = false;
    }
    

    IEnumerator CicleDamage()
    {
       
        isAttack = true;
        if (player != null)
        player.GetComponent<PlayerLife>().TakeDamage(damage);

        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }

  
}