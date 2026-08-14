using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class ShootEnemy : GenericEnemy
{
    [Header("Spawn Settings")]
    [SerializeField] GameObject shotPrefab;
    [SerializeField] float distacePlayer = 5;
    [SerializeField] int maxSpawnedEnemies = 4;

    private List<GameObject> activeSpawns = new List<GameObject>();
    private float _pathUpdateTimer;
    private const float PathUpdateInterval = 0.2f;

    protected override void Start()
    {
        base.Start();
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.stoppingDistance = distacePlayer; // seta uma vez só
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

    IEnumerator CicleDamage()
    {
        activeSpawns.RemoveAll(enemy => enemy == null);

        if (activeSpawns.Count < maxSpawnedEnemies)
        {
            GameObject newEnemy = Instantiate(shotPrefab, transform.position, Quaternion.identity);
            activeSpawns.Add(newEnemy);
        }

        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }

    protected override void Patrol()
    {
        agent.isStopped = false;
        base.Patrol();
    }

    protected override void Chase()
    {
        if (!feltPlayer || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > agent.stoppingDistance)
        {
            agent.isStopped = false;
            canAttack = false;

            // Atualiza rota só a cada PathUpdateInterval
            if (_pathUpdateTimer >= PathUpdateInterval)
            {
                _pathUpdateTimer = 0f;
                agent.SetDestination(player.transform.position);
            }
        }
        else
        {
            agent.isStopped = true;
            canAttack = true;
        }
    }
}