using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para recarregar a cena


public class PlayerLife : MonoBehaviour
{
    [SerializeField] float maxLife = 100f;
    private float currentLife;
    private PlayerCollect playerBag;
    public float LifePercent => currentLife / maxLife;
    [SerializeField] float maxOxygen; 
    float currentOxygen;
    public float oxygenPercent => currentOxygen / maxOxygen;

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
        print("O Jogador Morreu!");
        
        // Esvazia a mochila antes de morrer (como dita o GDD)
        if(playerBag != null)
        {
            playerBag.ClearBag();
        }

        // Simula o retorno à base recarregando a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}