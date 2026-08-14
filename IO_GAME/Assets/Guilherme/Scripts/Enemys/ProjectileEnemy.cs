using System.Collections;
using UnityEngine;

public class ProjectileEnemy : GenericEnemy
{
    [Header("Projétil")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootRange = 6f;

    private float _pathUpdateTimer;
    private const float PathUpdateInterval = 0.2f;

    protected override void Start()
    {
        base.Start();
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.stoppingDistance = 0f;
    }

    protected override void Update()
    {
        base.Update();

        _pathUpdateTimer += Time.deltaTime;

        if (canAttack && !isAttack)
        {
            isAttack = true;
            StartCoroutine(ShootCycle());
        }
    }

    protected override void Chase()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist <= shootRange)
        {
            agent.isStopped = true;
            canAttack = true;
        }
        else
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
    }

    public override void OnPlayerExitedArea()
    {
        canAttack = false;
        isAttack = false;
        agent.isStopped = false;
        base.OnPlayerExitedArea();
    }

    private IEnumerator ShootCycle()
    {
        if (player != null && projectilePrefab != null)
        {
            Vector3 origin = firePoint != null ? firePoint.position : transform.position;
            Vector2 direction = (player.transform.position - origin).normalized;

            GameObject proj = Instantiate(projectilePrefab, origin, Quaternion.identity);
            Projectil projectile = proj.GetComponent<Projectil>();
            if (projectile != null)
                projectile.Fire(direction, damage);
        }

        yield return new WaitForSeconds(cooldown);
        isAttack = false;
    }
}