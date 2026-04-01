using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShootEnemy : GenericEnemy
{
    [SerializeField] GameObject shotPrefab;

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
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > agent.stoppingDistance)
            {
                
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                canAttack = false;
            }
            else
            {
                
                agent.isStopped = true;
                canAttack = true;
            }
        }

        if (canAttack && !isAttack)
        {
            StartCoroutine(CicleDamage());
        }
    }

    IEnumerator CicleDamage()
    {
        isAttack = true;
        Instantiate(shotPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }
}