using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using DG.Tweening;

public class GenericEnemy : MonoBehaviour
{
    [System.Serializable]
    public struct TrashDrop
    {
        public GameObject prefab;
        public int amount;
    }

    public enum TypeDrop { fix, random }
    public TypeDrop type;
    public enum State { patrol, chase }
    public State currentState;

    protected NavMeshAgent agent;
    protected BoxCollider2D fisCollider;
    protected CircleCollider2D areaCollider;
    protected Rigidbody2D rb;
    protected PlayerMove player;

    public float life = 30f;
    public float damage = 10f;
    public float cooldown = 1f;

    [Header("Configuração de Drops")]
    [SerializeField] private List<TrashDrop> listTrashDrops;
    public int maxAmount;

    protected bool feltPlayer;
    protected bool isAttack;
    protected bool canAttack;
    bool haveAPoint = false;
    Vector2 randomPoint;

    [Header("Efeitos de Dano")]
    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private float knockbackForce = 4f;

    [Header("Efeitos de Morte")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float deathAnimDuration = 0.4f;
    // ─────────────────────────────────────────────
    // ÁREA
    // ─────────────────────────────────────────────

    private PollutedArea _area;
    private bool _playerInArea = false;

    // ─────────────────────────────────────────────
    // ANTI-STUCK (problema 3)
    // ─────────────────────────────────────────────

    private Vector3 _lastPosition;
    private float _stuckTimer = 0f;
    private const float StuckThreshold = 1f;    // segundos parado
    private const float StuckMinDistance = 0.05f; // distância mínima para considerar "movendo"

    public void SetArea(PollutedArea area)
    {
        _area = area;
    }

    public void OnPlayerEnteredArea()
    {
        _playerInArea = true;
    }

    public virtual void OnPlayerExitedArea()
    {
        _playerInArea = false;
        feltPlayer = false;
        currentState = State.patrol;
        haveAPoint = false;
    }

    /// <summary>
    /// Reavalia manualmente se o player já está na área/perto do inimigo.
    /// Usado pelo EnemyCuller ao reativar o inimigo (OnBecameVisible),
    /// já que enquanto o script estava "dormindo" ele pode ter perdido
    /// o evento de OnTriggerEnter2D do player.
    /// </summary>
    public virtual void RecheckPlayerProximity()
    {
        if (_playerInArea && player != null)
        {
            feltPlayer = true;
            currentState = State.chase;
        }
    }

    // ─────────────────────────────────────────────
    // LIFECYCLE
    // ─────────────────────────────────────────────

    protected virtual void Start()
    {
        currentState = State.patrol;
        player = FindObjectOfType<PlayerMove>();
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateUpAxis = false;
            agent.updateRotation = false;
        }

        _lastPosition = transform.position;
    }

    protected virtual void Update()
    {
        SwitchStates();
        CheckStuck();
    }

    // ─────────────────────────────────────────────
    // ANTI-STUCK
    // ─────────────────────────────────────────────

    private void CheckStuck()
    {
        if (currentState != State.patrol)
        {
            _stuckTimer = 0f;
            _lastPosition = transform.position;
            return;
        }

        float moved = Vector3.Distance(transform.position, _lastPosition);

        if (moved < StuckMinDistance)
        {
            _stuckTimer += Time.deltaTime;

            if (_stuckTimer >= StuckThreshold)
            {
                haveAPoint = false; // força novo ponto de patrulha
                _stuckTimer = 0f;
                Debug.Log($"[{gameObject.name}] Stuck detectado — novo ponto de patrulha.");
            }
        }
        else
        {
            _stuckTimer = 0f;
            _lastPosition = transform.position;
        }
    }

    // ─────────────────────────────────────────────
    // DANO E MORTE
    // ─────────────────────────────────────────────

    public virtual void TakeDamage(float amount)
    {
        life -= amount;

        SpawnDamageNumber(amount);
        StartCoroutine(FlashRed());
        StartCoroutine(Knockback());

        if (life <= 0) Die();
    }

    protected virtual void Die()
    {
        if (_area != null)
            _area.OnEnemyDied(this);

        // Spawna os drops imediatamente (não precisa esperar a animação)
        SpawnDrops();

        // Desliga a lógica do inimigo pra ele não continuar perseguindo/atacando enquanto morre
        if (agent != null) agent.enabled = false;
        if (fisCollider != null) fisCollider.enabled = false;
        if (areaCollider != null) areaCollider.enabled = false;

        StopAllCoroutines();

        // Toca o som de morte (funciona mesmo depois do objeto ser destruído)
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // Animação de morte: encolhe e faz fade, depois destrói
        transform.DOKill();

        Sequence deathSequence = DOTween.Sequence();
        deathSequence.Join(transform.DOScale(Vector3.zero, deathAnimDuration).SetEase(Ease.InBack));

        if (spriteRenderer != null)
        {
            deathSequence.Join(spriteRenderer.DOFade(0f, deathAnimDuration));
        }

        deathSequence.OnComplete(() => Destroy(gameObject));
    }

    private void SpawnDrops()
    {
        if (type == TypeDrop.fix)
        {
            foreach (TrashDrop drop in listTrashDrops)
            {
                if (drop.prefab != null)
                {
                    for (int i = 0; i < drop.amount; i++)
                    {
                        Vector3 offset = new Vector3(
                            Random.Range(-0.5f, 0.5f),
                            Random.Range(-0.5f, 0.5f), 0);
                        Instantiate(drop.prefab, transform.position + offset, Quaternion.identity);
                    }
                }
            }
        }
        else
        {
            List<GameObject> pool = new List<GameObject>();
            foreach (TrashDrop drop in listTrashDrops)
                if (drop.prefab != null) pool.Add(drop.prefab);

            if (pool.Count > 0 && maxAmount > 0)
            {
                for (int i = 0; i < maxAmount; i++)
                {
                    int idx = Random.Range(0, pool.Count);
                    Vector3 offset = new Vector3(
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(-0.5f, 0.5f), 0);
                    Instantiate(pool[idx], transform.position + offset, Quaternion.identity);
                }
            }
        }
    }

    // ─────────────────────────────────────────────
    // TRIGGERS
    // ─────────────────────────────────────────────

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (!_playerInArea) return;

        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            player = tempPlayer;
            feltPlayer = true;
            currentState = State.chase;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger) return;

        PlayerMove tempPlayer = collision.gameObject.GetComponent<PlayerMove>();
        if (tempPlayer != null)
        {
            feltPlayer = false;
            currentState = State.patrol;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) { }
    protected virtual void OnCollisionExit2D(Collision2D collision) { }

    // ─────────────────────────────────────────────
    // ESTADOS
    // ─────────────────────────────────────────────

    protected virtual void Chase()
    {
        if (agent != null && !agent.enabled) agent.enabled = true;
        if (player == null) return;

        Vector3 destination = player.transform.position;

        if (_area != null && !_area.ContainsPoint(destination))
            destination = _area.ClampToBounds(destination);

        agent.SetDestination(destination);
    }

    private float _patrolPathTimer;
    private const float PatrolPathInterval = 0.3f;

    protected virtual void Patrol()
    {
        if (agent != null && !agent.enabled) agent.enabled = true;
        agent.isStopped = false;

        if (!haveAPoint)
        {
            if (_area != null)
            {
                Bounds b = _area.Bounds;
                randomPoint = new Vector2(
                    Random.Range(b.min.x, b.max.x),
                    Random.Range(b.min.y, b.max.y));
            }
            else
            {
                randomPoint = new Vector2(
                    transform.position.x + Random.Range(-5f, 5f),
                    transform.position.y + Random.Range(-5f, 5f));
            }

            haveAPoint = true;
            agent.SetDestination(randomPoint); // seta imediatamente ao escolher novo ponto
            _patrolPathTimer = 0f;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                haveAPoint = false;
        }
    }

    protected virtual void SwitchStates()
    {
        switch (currentState)
        {
            case State.patrol: Patrol(); break;
            case State.chase: Chase(); break;
        }
    }

    // ─────────────────────────────────────────────
    // EFEITOS
    // ─────────────────────────────────────────────

    private void SpawnDamageNumber(float amount)
    {
        if (damageNumberPrefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-0.3f, 0.3f), 0.5f, 0);

        GameObject obj = Instantiate(damageNumberPrefab, spawnPos, Quaternion.identity);
        obj.GetComponent<DamageNumber>().Init(amount);
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer == null) yield break;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator Knockback()
    {
        if (agent == null || player == null) yield break;

        agent.isStopped = true;
        agent.updatePosition = false;

        Vector3 direction = (transform.position - player.transform.position).normalized;
        Vector3 targetPos = transform.position + direction * 0.6f;

        if (_area != null)
            targetPos = _area.ClampToBounds(targetPos);

        yield return transform.DOMove(targetPos, 0.08f)
            .SetEase(Ease.OutQuad)
            .WaitForCompletion();

        agent.Warp(transform.position);
        agent.updatePosition = true;
        agent.isStopped = false;
    }
}