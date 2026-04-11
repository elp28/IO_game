    using UnityEngine;
    using UnityEngine.AI;
    using System.Collections;

    public class ShootEnemy : GenericEnemy
    {
        [SerializeField] GameObject shotPrefab;
        [SerializeField] float distacePlayer = 5;

        protected override void Start()
        {
            base.Start();
            fisCollider = GetComponent<BoxCollider2D>();
            areaCollider = GetComponentInChildren<CircleCollider2D>();
            rb = GetComponent<Rigidbody2D>();
            agent = GetComponent<NavMeshAgent>();
            agent.updateUpAxis = false;
            agent.updateRotation = false;
            DistacePlayer();
        }

        protected override void Update()
        {
            base.Update();
            if (!IsFree) { return; }

            if(currentState != GenericEnemy.State.patrol) DistacePlayer();

            if (canAttack && !isAttack)
            {
                isAttack = true;
                StartCoroutine(CicleDamage());
            }
        }

        IEnumerator CicleDamage()
        { 
            Instantiate(shotPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(cooldown);
            isAttack = false;
        }

        protected override void Patrol()
        {
            agent.isStopped = false;
            agent.stoppingDistance = 0;
            base.Patrol();

        }

        protected override void Chase()
        {
            base.Chase();
            
            if (feltPlayer && player != null)
            {

                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

                if (distanceToPlayer > agent.stoppingDistance)
                {

                    agent.isStopped = false;
                    if (player != null)
                        agent.SetDestination(player.transform.position);

                    canAttack = false;
                }
                else
                {
                    agent.isStopped = true;
                    canAttack = true;
                }
            }
        }

        protected override void Caught()
        {
            base.Caught();
        }

        void DistacePlayer()
        {
            agent.stoppingDistance = distacePlayer;
        }
    }