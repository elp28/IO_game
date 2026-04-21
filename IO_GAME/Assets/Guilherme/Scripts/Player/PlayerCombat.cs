using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Criamos uma lista de armas disponíveis (Enum)
    public enum WeaponType { Harpoon, Laser, Bomba } // Adicione novas aqui no futuro
    
    [Header("Sistema de Armas")]
    public WeaponType currentWeapon = WeaponType.Harpoon; // Arma selecionada atualmente
    [SerializeField] private float cooldown = 0.5f;
    private float nextFireTime = 0f;

    [Header("Arma: Arpão")]
    [SerializeField] private GameObject harpoonPrefab;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float fireForce = 12f;
    
    [Header("Mira Automática")]
    [SerializeField] private float detectionRadius = 8f; // Tamanho do "radar"
    [SerializeField] private LayerMask enemyLayer; // Define o que é inimigo para o radar

    [Header("Teste no PC")]
    [SerializeField] private KeyCode attackKey = KeyCode.Space;
    [SerializeField] private KeyCode switchWeaponKey = KeyCode.Q; // Tecla de teste para trocar arma

    void Update()
    {
        // Atirar
        if (Input.GetKeyDown(attackKey))
        {
            Shoot();
        }

        // Teste: Trocar de arma rápido pelo teclado
        if (Input.GetKeyDown(switchWeaponKey))
        {
            SwitchToNextWeapon();
        }
    }

    // Função central que decide qual arma atirar
    public void Shoot()
    {
        if (Time.time < nextFireTime) return; // Trava do cooldown
        nextFireTime = Time.time + cooldown;

        // O switch direciona o código para a função da arma correta
        switch (currentWeapon)
        {
            case WeaponType.Harpoon:
                ShootHarpoon();
                break;
            case WeaponType.Laser:
                // Exemplo: ShootLaser(); 
                Debug.Log("Atirando com o Laser! (Falta criar o prefab)");
                break;
            case WeaponType.Bomba:
                // Exemplo: ThrowBomb();
                Debug.Log("Jogando Bomba! (Falta criar o prefab)");
                break;
        }
    }

    // A lógica específica do arpão fica isolada aqui
    private void ShootHarpoon()
    {
        if (harpoonPrefab == null || firePoint == null) return;

        GameObject harpoon = Instantiate(harpoonPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = harpoon.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            Transform target = GetNearestEnemy();

            if (target != null)
            {
                // Calcula a direção exata até o inimigo
                Vector2 direction = (target.position - firePoint.position).normalized;
                
                // Dispara o arpão naquela direção
                rb.linearVelocity = direction * fireForce;
                
                // Gira o sprite do arpão para apontar para o inimigo
                harpoon.transform.right = direction; 
            }
            else
            {
                // Se não tiver inimigo na área, atira reto para onde o player está olhando
                rb.linearVelocity = firePoint.right * fireForce;
            }
        }
    }

    // O nosso "Radar Invisível"
    private Transform GetNearestEnemy()
    {
        // Cria um círculo invisível que acha todos os colliders que estão na Layer "Enemy"
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        // Passa por todos os inimigos encontrados e verifica qual é o mais perto
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

    // Função bônus para ciclar entre as armas
    public void SwitchToNextWeapon()
    {
        // Isso aqui pula para a próxima arma da lista Enum (só para facilitar seus testes)
        int nextWeaponIndex = ((int)currentWeapon + 1) % System.Enum.GetValues(typeof(WeaponType)).Length;
        currentWeapon = (WeaponType)nextWeaponIndex;
        Debug.Log("Arma trocada para: " + currentWeapon);
    }

    // Desenha o radar na tela do Unity para você poder configurar o tamanho
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}