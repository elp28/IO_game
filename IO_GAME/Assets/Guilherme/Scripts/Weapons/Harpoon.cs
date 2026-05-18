using UnityEngine;
using DG.Tweening;

public class Harpoon : MonoBehaviour
{
    // Dano não é mais serializado — recebido do PlayerCombat no momento do disparo
    private float damage;

    [SerializeField] private float returnSpeed = 30f;
    [SerializeField] private LineRenderer lineRenderer;

    private Transform firePoint;
    private bool isReturning = false;
    private Tween moveTween;

    /// <summary>
    /// Chamado pelo PlayerCombat logo após instanciar o arpão.
    /// </summary>
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    /// <summary>
    /// Inicia o disparo em direção ao alvo.
    /// </summary>
    public void Fire(Transform origin, Vector3 targetPosition, float speed)
    {
        firePoint = origin;
        isReturning = false;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }

        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / speed;

        moveTween = transform.DOMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => ReturnToPlayer());
    }

    void Update()
    {
        if (lineRenderer != null && firePoint != null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, transform.position);
        }

        if (isReturning && firePoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, firePoint.position, returnSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, firePoint.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
        else if (isReturning && firePoint == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger) return;
        if (collision.CompareTag("Player") || collision.CompareTag("Trash")) return;

        GenericEnemy enemy = collision.GetComponent<GenericEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (!isReturning)
        {
            ReturnToPlayer();
        }
    }

    private void ReturnToPlayer()
    {
        if (isReturning) return;
        isReturning = true;

        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }

    private void OnDestroy()
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }
    }
}