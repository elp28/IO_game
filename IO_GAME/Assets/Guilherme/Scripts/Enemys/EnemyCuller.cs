using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Coloque este script no mesmo GameObject do inimigo.
/// Desativa componentes pesados quando fora da câmera.
/// Usa OnBecameVisible/OnBecameInvisible do SpriteRenderer — zero overhead.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyCuller : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Rigidbody2D _rb;
    private GenericEnemy _enemy;
    private bool _isVisible = true;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody2D>();
        _enemy = GetComponent<GenericEnemy>();
    }

    /// <summary>
    /// Chamado automaticamente pelo Unity quando o SpriteRenderer
    /// entra no frustum de qualquer câmera.
    /// </summary>
    void OnBecameVisible()
    {
        if (_isVisible) return;
        _isVisible = true;

        if (_agent != null) _agent.enabled = true;
        if (_rb != null) _rb.simulated = true;
        if (_enemy != null) _enemy.enabled = true;
    }

    /// <summary>
    /// Chamado automaticamente pelo Unity quando o SpriteRenderer
    /// sai do frustum de todas as câmeras.
    /// </summary>
    void OnBecameInvisible()
    {
        if (!_isVisible) return;
        _isVisible = false;

        if (_agent != null) _agent.enabled = false;
        if (_rb != null) _rb.simulated = false;
        if (_enemy != null) _enemy.enabled = false;
    }
}