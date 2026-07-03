using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] float maxLife = 100f;
    private float currentLife;
    private PlayerCollect playerBag;
    public float LifePercent => currentLife / maxLife;

    [SerializeField] float maxOxygen;
    float currentOxygen;
    public float oxygenPercent => currentOxygen / maxOxygen;

    [SerializeField] private GameObject damageNumberPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.12f;

    [Header("Regeneração Passiva")]
    [Tooltip("Segundos sem tomar dano para começar a regenerar.")]
    [SerializeField] private float regenDelay = 5f;
    [Tooltip("HP regenerado por segundo fora da estação.")]
    [SerializeField] private float regenPerSecond = 2f;

    [Header("Regeneração na Estação")]
    [Tooltip("HP regenerado por segundo dentro da estação.")]
    [SerializeField] private float stationLifeRegenPerSecond = 20f;
    [Tooltip("Oxigênio recuperado por segundo dentro da estação.")]
    [SerializeField] private float stationOxyRegenPerSecond = 40f;

    bool isAtStation;
    private float _timeSinceLastDamage;

    void Start()
    {
        currentLife = maxLife;
        currentOxygen = maxOxygen;
        playerBag = GetComponent<PlayerCollect>();
        _timeSinceLastDamage = regenDelay; // começa podendo regenerar
    }

    void Update()
    {
        if (isAtStation)
        {
            RegenAtStation();
            return;
        }

        // Oxigênio cai fora da estação
        if (currentOxygen > 0)
            currentOxygen -= Time.deltaTime;
        else
            Die();

        // Regeneração passiva após delay
        _timeSinceLastDamage += Time.deltaTime;
        if (_timeSinceLastDamage >= regenDelay && currentLife < maxLife)
        {
            currentLife = Mathf.Min(currentLife + regenPerSecond * Time.deltaTime, maxLife);
        }
    }

    private void RegenAtStation()
    {
        // Vida sobe rapidamente
        if (currentLife < maxLife)
            currentLife = Mathf.Min(currentLife + stationLifeRegenPerSecond * Time.deltaTime, maxLife);

        // Oxigênio sobe rapidamente
        if (currentOxygen < maxOxygen)
            currentOxygen = Mathf.Min(currentOxygen + stationOxyRegenPerSecond * Time.deltaTime, maxOxygen);
    }

    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        _timeSinceLastDamage = 0f; // reseta o delay de regeneração

        print("Vida do Jogador: " + currentLife);
        SpawnDamageNumber(damage);
        StartCoroutine(FlashRed());

        if (currentLife <= 0)
            Die();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        BoxCollect boxCollect = collider.gameObject.GetComponent<BoxCollect>();
        if (boxCollect != null)
            isAtStation = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        BoxCollect boxCollect = collider.gameObject.GetComponent<BoxCollect>();
        if (boxCollect != null)
            isAtStation = false;
    }

    void Die()
    {
        if (playerBag != null)
            playerBag.ClearBag();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SpawnDamageNumber(float amount)
    {
        if (damageNumberPrefab == null) return;

        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), 0.5f, 0);
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

    // ─────────────────────────────────────────────
    // API DE UPGRADES
    // ─────────────────────────────────────────────

    public void SetMaxLife(float newMax)
    {
        float ratio = currentLife / maxLife;
        maxLife = newMax;
        currentLife = Mathf.Clamp(maxLife * ratio, 0f, maxLife);
    }

    public void SetMaxOxygen(float newMax)
    {
        maxOxygen = newMax;
        currentOxygen = maxOxygen;
    }
}