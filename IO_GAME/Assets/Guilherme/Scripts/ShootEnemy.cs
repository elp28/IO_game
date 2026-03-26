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
        if (feltPlayer && player != null)
        {
            // Calcula a distância real matematicamente
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > agent.stoppingDistance)
            {
                // Player está longe: anda até ele
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
                canAttack = false;
            }
            else
            {
                // Player está perto: para de andar e começa a atirar
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

        // Troquei o número "5" fixo pela variável cooldown que você já tem na classe base!
        yield return new WaitForSeconds(cooldown);

        isAttack = false;
    }
}