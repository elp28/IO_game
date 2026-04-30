using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections.Generic;
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

    bool isAtStation;
    
    void Start()
    {
        currentLife = maxLife;
        currentOxygen = maxOxygen;
        playerBag = GetComponent<PlayerCollect>();
    }

    void Update()
    {
        if(isAtStation) return;
        
        if(currentOxygen > 0)
        {
            currentOxygen -= Time.deltaTime;
        }
        else
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        print("Vida do Jogador: " + currentLife);
        SpawnDamageNumber(damage);    
        StartCoroutine(FlashRed());


        if (currentLife <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        BoxCollect boxCollect = collider.gameObject.GetComponent<BoxCollect>();
        if(boxCollect != null)
        {
            ResetOxygen();
            isAtStation = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        BoxCollect boxCollect = collider.gameObject.GetComponent<BoxCollect>();
        if(boxCollect != null)
        {
            isAtStation = false;
        }
    }

    void ResetOxygen()
    {
        currentOxygen = maxOxygen;
    }
    void Die()
    {
        if(playerBag != null)
        {
            playerBag.ClearBag();
        }
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
}