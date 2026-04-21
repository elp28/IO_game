using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Configurações da Arma")]
    [SerializeField] private GameObject harpoonPrefab;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float fireForce = 12f;
    [SerializeField] private float cooldown = 0.5f;

    [Header("Teste no PC")]
    [SerializeField] private KeyCode attackKey = KeyCode.Space;

    private float nextFireTime = 0f;

    void Update()
    {
        // REATIVADO: Teste se apertando ESPAÇO o tiro sai.
        // Se sair pelo teclado e não pelo botão, o problema é no Canvas.
        if (Input.GetKeyDown(attackKey))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (Time.time >= nextFireTime)
        {
            if (harpoonPrefab != null && firePoint != null)
            {
                nextFireTime = Time.time + cooldown;
                
                GameObject harpoon = Instantiate(harpoonPrefab, firePoint.position, firePoint.rotation);
                
                Rigidbody2D rb = harpoon.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // O tiro sai para onde o FirePoint estiver apontando (X vermelho)
                    rb.velocity = firePoint.right * fireForce; 
                }
            }
            else
            {
                Debug.LogWarning("Faltam referências no Inspector do Player!");
            }
        }
    }
}