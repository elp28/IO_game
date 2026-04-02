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
        
        // Se NÃO está livre (sugado), para de agir normalmente
        if (!IsFree) { return; } 

        if (feltPlayer && player != null)
        {
            agent.SetDestination(player.transform.position);
        }

        // CORREÇÃO: Removido o !IsFree daqui para ele conseguir atacar
        if (canAttack && !isAttack)
        {
            isAttack = true;
            StartCoroutine(CicleDamage());
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // Roda a lógica de ser destruído caso esteja sendo sugado
        base.OnCollisionEnter2D(collision);
        
        // CORREÇÃO: Se ele NÃO estiver livre (sugado), ignora o resto
        if (!IsFree) { return; } 

        // Proteção do NavMesh
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
        
        // CORREÇÃO: Se ele NÃO estiver livre, ignora o resto
        if (!IsFree) { return; } 

        // Proteção do NavMesh
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
}