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
    [SerializeField] private float regenDelay = 5f;
    [SerializeField] private float regenPerSecond = 2f;

    [Header("Regeneração na Estação")]
    [SerializeField] private float stationLifeRegenPerSecond = 20f;
    [SerializeField] private float stationOxyRegenPerSecond = 40f;

    bool isAtStation;
    private float _timeSinceLastDamage;

    void Start()
    {
        currentLife = maxLife;
        currentOxygen = maxOxygen;
        playerBag = GetComponent<PlayerCollect>();
        _timeSinceLastDamage = regenDelay;
    }

    void Update()
    {
        if (isAtStation)
        {
            RegenAtStation();
            return;
        }

        if (currentOxygen > 0)
            currentOxygen -= Time.deltaTime;
        else
            Die();

        _timeSinceLastDamage += Time.deltaTime;
        if (_timeSinceLastDamage >= regenDelay && currentLife < maxLife)
            currentLife = Mathf.Min(currentLife + regenPerSecond * Time.deltaTime, maxLife);
    }

    private void RegenAtStation()
    {
        if (currentLife < maxLife)
            currentLife = Mathf.Min(currentLife + stationLifeRegenPerSecond * Time.deltaTime, maxLife);

        if (currentOxygen < maxOxygen)
            currentOxygen = Mathf.Min(currentOxygen + stationOxyRegenPerSecond * Time.deltaTime, maxOxygen);
    }

    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        _timeSinceLastDamage = 0f;

        SpawnDamageNumber(damage);
        StartCoroutine(FlashRed());

        if (currentLife <= 0)
            Die();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        BoxCollect boxCollect = collider.gameObject.GetComponent<BoxCollect>();
        if (boxCollect != null) isAtStation = true;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        BoxCollect boxCollect = collider.gameObject.GetComponent<BoxCollect>();
        if (boxCollect != null) isAtStation = false;
    }

    void Die()
    {
        if (playerBag != null)
            playerBag.ClearBag();

        GameManager.instance.TriggerGameOver();
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