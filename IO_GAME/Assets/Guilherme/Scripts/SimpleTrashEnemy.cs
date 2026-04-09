using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleTrashEnemy : GenericEnemy
{
    protected override void Start()
    {
        base.Start();
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    protected override void Update()
    {
        base.Update();
        
       
        if (!IsFree) { return; } 

        if (canAttack && !isAttack)
        {
            isAttack = true;
            StartCoroutine(CicleDamage());
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
     
        base.OnCollisionEnter2D(collision);
        
     
        if (!IsFree) { return; } 


        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
            canAttack = true;
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        
  
        if (!IsFree) { return; } 

   
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
        
        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
            canAttack = false;
    }
    
    IEnumerator CicleDamage()
    {
        if (player != null)
            player.GetComponent<PlayerLife>().TakeDamage(damage);

        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }

    protected override void Caught()
    {
        base.Caught();
    }

    protected override void Chase()
    {
        base.Chase();      
        agent.SetDestination(player.transform.position);
    }

    protected override void Patrol()
    {
        base.Patrol();
    }
}