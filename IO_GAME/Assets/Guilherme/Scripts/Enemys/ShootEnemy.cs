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

            if(currentState != GenericEnemy.State.patrol) DistacePlayer();

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

        void DistacePlayer()
        {
            agent.stoppingDistance = distacePlayer;
        }
    }