using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SimpleTrashEnemy : GenericEnemy
{
    private PlayerLife _playerLife;
    private float _pathUpdateTimer;
    private const float PathUpdateInterval = 0.2f; // atualiza rota 5x por segundo

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

        _pathUpdateTimer += Time.deltaTime;

        if (canAttack && !isAttack)
        {
            isAttack = true;
            StartCoroutine(CicleDamage());
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.isStopped = true;

        PlayerLife playerLife = collision.gameObject.GetComponent<PlayerLife>();
        if (playerLife != null)
        {
            _playerLife = playerLife;
            canAttack = true;
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);

        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (collision.gameObject.GetComponent<PlayerLife>() != null)
            canAttack = false;
    }

    IEnumerator CicleDamage()
    {
        // Usa referência cacheada em vez de GetComponent todo ciclo
        if (_playerLife != null)
            _playerLife.TakeDamage(damage);

        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }

    protected override void Chase()
    {
        // Atualiza rota só a cada PathUpdateInterval — não todo frame
        if (_pathUpdateTimer >= PathUpdateInterval)
        {
            _pathUpdateTimer = 0f;
            base.Chase();
            if (player != null)
                agent.SetDestination(player.transform.position);
        }
    }

    protected override void Patrol()
    {
        base.Patrol();
    }
}