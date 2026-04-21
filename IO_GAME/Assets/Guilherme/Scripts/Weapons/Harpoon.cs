using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifeTime = 2f; 

    void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player") || collision.CompareTag("Trash")) return;

        
        GenericEnemy enemy = collision.GetComponent<GenericEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); 
        }
    }
}