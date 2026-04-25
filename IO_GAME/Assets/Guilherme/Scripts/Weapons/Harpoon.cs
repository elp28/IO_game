using UnityEngine;
using DG.Tweening; // Necessário para o DoTween

public class Harpoon : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float returnSpeed = 30f; // Velocidade que ele volta para o player
    [SerializeField] private LineRenderer lineRenderer; // O fio do arpão

    private Transform firePoint;
    private bool isReturning = false;
    private Tween moveTween;

    // Método chamado pelo PlayerCombat para iniciar o tiro
    public void Fire(Transform origin, Vector3 targetPosition, float speed)
    {
        firePoint = origin;
        isReturning = false;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2; // Garante que o fio tem dois pontos (início e fim)
        }

        // Calcula o tempo que o DoTween vai levar com base na distância e velocidade
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / speed;

        // Animação de ida com DoTween
        moveTween = transform.DOMove(targetPosition, duration)
            .SetEase(Ease.Linear) // Linear para parecer velocidade constante
            .OnComplete(() => ReturnToPlayer()); // Quando chegar no destino, volta
    }

    void Update()
    {
        // 1. Atualiza o visual do fio
        if (lineRenderer != null && firePoint != null)
        {
            lineRenderer.SetPosition(0, firePoint.position); // Ponto 0 preso na arma
            lineRenderer.SetPosition(1, transform.position); // Ponto 1 preso no arpão
        }

        // 2. Lógica de Volta
        // Usamos o Update e não o DoTween para a volta porque o Player pode estar se movendo!
        if (isReturning && firePoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, firePoint.position, returnSpeed * Time.deltaTime);

            // Se chegou perto o suficiente do player, o arpão é destruído (recolhido)
            if (Vector3.Distance(transform.position, firePoint.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
        else if (isReturning && firePoint == null)
        {
            // Proteção caso o player seja destruído enquanto o arpão está no ar
            Destroy(gameObject);
        }
    }

   private void OnTriggerEnter2D(Collider2D collision)
{
    // 1. O FILTRO MÁGICO: Se for um gatilho (área invisível), o arpão ignora e passa direto
    if (collision.isTrigger) return;

    // 2. Ignora player ou lixo (tags de segurança)
    if (collision.CompareTag("Player") || collision.CompareTag("Trash")) return;

    // Aplica dano se for inimigo
    GenericEnemy enemy = collision.GetComponent<GenericEnemy>();
    if (enemy != null)
    {
        enemy.TakeDamage(damage);
    }

    // Se chegou aqui, é algo sólido (parede, inimigo, chão). Volta!
    if (!isReturning)
    {
        ReturnToPlayer();
    }
}

    private void ReturnToPlayer()
    {
        if (isReturning) return; // Evita chamar duas vezes
        isReturning = true;

        // Cancela a animação de ida do DoTween
        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }

    // Segurança: limpa a memória do DoTween se o arpão for destruído abruptamente
    private void OnDestroy()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }
}