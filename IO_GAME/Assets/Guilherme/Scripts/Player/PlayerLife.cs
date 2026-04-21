using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para recarregar a cena

public class PlayerLife : MonoBehaviour
{
    [SerializeField] float maxLife = 100f;
    private float currentLife;
    private PlayerCollect playerBag;

    void Start()
    {
        currentLife = maxLife;
        playerBag = GetComponent<PlayerCollect>();
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