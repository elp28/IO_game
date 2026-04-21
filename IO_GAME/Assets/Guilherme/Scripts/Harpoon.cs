using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 2f; // Some se não acertar nada

    void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignora o próprio jogador e o lixo no chão
        if (collision.CompareTag("Player") || collision.CompareTag("Trash")) return;

        // Se acertou um inimigo
        GenericEnemy enemy = collision.GetComponent<GenericEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // Destrói o arpão
        }
    }
}