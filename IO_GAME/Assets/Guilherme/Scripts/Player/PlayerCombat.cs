using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public enum WeaponType { Harpoon, Laser, Bomba }
    
    [Header("Sistema de Armas")]
    public WeaponType currentWeapon = WeaponType.Harpoon;
    [SerializeField] private float cooldown = 0.5f;
    private float nextFireTime = 0f;

    [Header("Arma: Arpão")]
    [SerializeField] private GameObject harpoonPrefab;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float fireForce = 20f; // Usaremos como "Velocidade" agora
    
    [Header("Mira Automática")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Teste no PC")]
    [SerializeField] private KeyCode attackKey = KeyCode.Space;
    [SerializeField] private KeyCode switchWeaponKey = KeyCode.Q;

    void Update()
    {
        if (Input.GetKeyDown(attackKey)) Shoot();
        if (Input.GetKeyDown(switchWeaponKey)) SwitchToNextWeapon();
    }

    public void Shoot()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + cooldown;

        switch (currentWeapon)
        {
            case WeaponType.Harpoon:
                ShootHarpoon();
                break;
            case WeaponType.Laser:
                Debug.Log("Atirando com o Laser!");
                break;
            case WeaponType.Bomba:
                Debug.Log("Jogando Bomba!");
                break;
        }
    }

    private void ShootHarpoon()
    {
        if (harpoonPrefab == null || firePoint == null) return;

        GameObject harpoonObj = Instantiate(harpoonPrefab, firePoint.position, firePoint.rotation);
        Harpoon harpoonScript = harpoonObj.GetComponent<Harpoon>();
        
        if (harpoonScript != null)
        {
            Transform target = GetNearestEnemy();
            Vector3 targetPosition;

            if (target != null)
            {
                // Calcula a direção e define o alvo exatamente no inimigo
                Vector2 direction = (target.position - firePoint.position).normalized;
                targetPosition = target.position;
                harpoonObj.transform.right = direction; 
            }
            else
            {
                // Se não tem inimigo, atira reto até o limite do radar
                targetPosition = firePoint.position + (firePoint.right * detectionRadius);
            }

            // Dispara passando quem é a origem, para onde vai, e a velocidade
            harpoonScript.Fire(firePoint, targetPosition, fireForce);
        }
    }

    private Transform GetNearestEnemy()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in hitEnemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }
        return nearestEnemy;
    }

    public void SwitchToNextWeapon()
    {
        int nextWeaponIndex = ((int)currentWeapon + 1) % System.Enum.GetValues(typeof(WeaponType)).Length;
        currentWeapon = (WeaponType)nextWeaponIndex;
        Debug.Log("Arma trocada para: " + currentWeapon);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}