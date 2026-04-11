    using System.Collections;
    using UnityEngine;
    using UnityEngine.AI;

    public class GenericEnemy : MonoBehaviour
    {
        public enum TypeEnemy { contact, shooter}
        public TypeEnemy type;
        public enum State { patrol, chase, caught}
        public State currentState;
        protected NavMeshAgent agent;
        protected BoxCollider2D fisCollider;
        protected CircleCollider2D areaCollider;
        protected Rigidbody2D rb;
        protected bool feltPlayer;
        public float life;
        public float damage;
        protected PlayerMove player;
        protected bool isAttack;
        protected bool canAttack;
        public float cooldown;
        [SerializeField] bool isFree;
        public bool IsFree => isFree;
        public float pullSpeed = 10f; 
        PlayerCollect bagOfPlayer;
        bool haveAPoint = false;

         float randomValueX;
            float randomValueY;
            Vector2 randomPoint;
        
        protected virtual void Start()
        {
            isFree = true;
            currentState = State.patrol;
            player = FindObjectOfType<PlayerMove>(); 
        }

        protected virtual void Update()
        {
            SwitchStates();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
            if (tempPlayer != null)
            {
                player = tempPlayer;
                feltPlayer = true;
                currentState = State.chase;
                print(currentState);
            }

        }

        
        protected virtual void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.isTrigger) return;

            PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
            if (tempPlayer != null)
            {
                feltPlayer = true;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.isTrigger) return;

            PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
            if (tempPlayer != null)
            {
                feltPlayer = false;
                player = null;
                currentState = State.patrol;
                print(currentState);
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
            if (tempPlayer != null)
            {
                if (!isFree)
                {
                    bagOfPlayer.PickedTrash();
                    Die();
                }
            }
        }

        protected virtual void OnCollisionExit2D(Collision2D collision)
        {

        }

        public virtual void ItsOverForTrash(bool state, PlayerCollect playerBag)
        {
            currentState = State.caught;
            print(currentState);
            isFree = state;
            bagOfPlayer = playerBag;
        }

        protected virtual void Die()
        {
            Destroy(gameObject);
        }

        protected virtual void Chase()
        {
            if (agent != null && !agent.enabled)
                agent.enabled = true;
        }

        protected virtual void Patrol()
        {
           
            if (agent != null && !agent.enabled) agent.enabled = true;

            agent.isStopped = false;
            if(!haveAPoint)
            {
                randomValueX = Random.Range(-5, 5f);
                randomValueY = Random.Range(-5f, 5f);
                randomPoint = new Vector2(transform.localPosition.x - randomValueX, transform.localPosition.y - randomValueY);
                haveAPoint = true;
   
            }

            agent.SetDestination(randomPoint);
            if (!agent.pathPending) // O agente já terminou de calcular a rota?
            {
                if (agent.remainingDistance <= agent.stoppingDistance) // A distância restante é menor que a de parada?
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) // Ele não tem mais caminho ou parou de se mover?
                    {
                        haveAPoint = false;
                    }
                }
            }

        }

        protected virtual void Caught()
        {
            if (agent != null && agent.enabled) agent.enabled = false;

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, pullSpeed * Time.deltaTime);
        }

        protected virtual void SwitchStates()
        {
           
            switch (currentState)
            {
                
                case State.patrol:
                    Patrol();
                    break;

                case State.chase:
                    Chase();
                    haveAPoint = false;
                    break;

                case State.caught:
                    Caught();
                    haveAPoint = false;
                    break;
            }
        }
    }