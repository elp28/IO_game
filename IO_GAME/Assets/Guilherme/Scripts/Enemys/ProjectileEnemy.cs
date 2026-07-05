using System.Collections;
using UnityEngine;

public class ProjectileEnemy : GenericEnemy
{
    [Header("Projétil")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform  firePoint;
    [SerializeField] private float      shootRange = 6f;

    protected override void Start()
    {
        base.Start();
        fisCollider  = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb           = GetComponent<Rigidbody2D>();
        agent        = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateUpAxis     = false;
        agent.updateRotation   = false;
        agent.stoppingDistance = 0f; // não interfere — controlamos nós mesmos
    }

    protected override void Update()
    {
        base.Update();

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
            // No alcance — para e atira
            agent.isStopped = true;
            canAttack = true;
        }
        else
        {
            // Fora do alcance — persegue
            agent.isStopped = false;
            canAttack = false;
            agent.SetDestination(player.transform.position);
        }
    }

    public override void OnPlayerExitedArea()
    {
        canAttack = false;
        isAttack  = false;
        agent.isStopped = false;
        base.OnPlayerExitedArea();
    }

    private IEnumerator ShootCycle()
    {
        if (player != null && projectilePrefab != null)
        {
            Vector3 origin    = firePoint != null ? firePoint.position : transform.position;
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