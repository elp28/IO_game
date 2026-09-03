using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Coloque este script no mesmo GameObject do inimigo.
/// Desativa componentes pesados (NavMeshAgent, física) quando fora da câmera.
/// Usa OnBecameVisible/OnBecameInvisible do SpriteRenderer — zero overhead.
///
/// IMPORTANTE: NÃO desativamos mais o GenericEnemy inteiro. Se fizermos isso,
/// o Update() (onde roda o SwitchStates) e o OnTriggerEnter2D param de
/// disparar enquanto o inimigo está fora da tela — se o player entrar na
/// área de detecção nesse período, o evento é perdido e o inimigo nunca
/// mais persegue, mesmo depois de voltar a ficar visível. Por isso, ao
/// voltar a ficar visível, forçamos uma reavaliação manual via
/// RecheckPlayerProximity().
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

        // Reavalia se o player já está por perto/na área, já que enquanto
        // o inimigo estava "dormindo" ele pode ter perdido o OnTriggerEnter2D.
        if (_enemy != null) _enemy.RecheckPlayerProximity();
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

        // GenericEnemy.enabled continua true de propósito: o Update() dele é
        // leve (SwitchStates + CheckStuck) e precisa continuar rodando para
        // que OnTriggerEnter2D/Exit2D não percam eventos de detecção do player.
    }
}