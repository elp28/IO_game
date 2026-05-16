using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Guarda os níveis atuais de cada upgrade e aplica os efeitos nos sistemas do jogador.
///
/// NÃO conhece a loja. Recebe apenas: "Aplique este upgrade."
/// A loja chama ApplyUpgrade(UpgradeData) e para por aí.
/// </summary>
public class PlayerUpgradeManager : MonoBehaviour
{
    public static PlayerUpgradeManager instance;

    [Header("Referências do Jogador")]
    [SerializeField] private PlayerLife    playerLife;
    [SerializeField] private PlayerCollect playerCollect;
    [SerializeField] private PlayerCombat  playerCombat;

    // ─────────────────────────────────────────────
    // ESTADO INTERNO — níveis atuais por upgrade
    // ─────────────────────────────────────────────

    // Chave: o asset UpgradeData | Valor: nível atual (0 = não comprado)
    private Dictionary<UpgradeData, int> _levels = new Dictionary<UpgradeData, int>();

    // ─────────────────────────────────────────────
    // LIFECYCLE
    // ─────────────────────────────────────────────

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // ─────────────────────────────────────────────
    // API PÚBLICA — a loja só usa esses dois métodos
    // ─────────────────────────────────────────────

    /// <summary>
    /// Retorna o nível atual de um upgrade. 0 = não comprado.
    /// </summary>
    public int GetLevel(UpgradeData upgrade)
    {
        return _levels.TryGetValue(upgrade, out int level) ? level : 0;
    }

    /// <summary>
    /// Sobe um nível no upgrade e aplica o efeito imediatamente.
    /// Chamado pela loja após confirmar pagamento.
    /// </summary>
    public void ApplyUpgrade(UpgradeData upgrade)
    {
        int currentLevel = GetLevel(upgrade);

        if (upgrade.IsMaxLevel(currentLevel))
        {
            Debug.LogWarning($"[Upgrades] {upgrade.upgradeName} já está no nível máximo.");
            return;
        }

        int newLevel = currentLevel + 1;
        _levels[upgrade] = newLevel;

        float newValue = upgrade.GetValueAtLevel(newLevel);

        ApplyEffect(upgrade, newValue);

        Debug.Log($"[Upgrades] {upgrade.upgradeName} → Nível {newLevel} | Valor aplicado: {newValue}");
    }

    // ─────────────────────────────────────────────
    // APLICAÇÃO DOS EFEITOS
    // ─────────────────────────────────────────────

    private void ApplyEffect(UpgradeData upgrade, float value)
    {
        switch (upgrade.category)
        {
            case UpgradeCategory.Survival when upgrade.upgradeName == "Vida":
                ApplyVida(value);
                break;

            case UpgradeCategory.Survival when upgrade.upgradeName == "Oxigênio":
                ApplyOxigenio(value);
                break;

            case UpgradeCategory.Combat:
                ApplyDano(value);
                break;

            case UpgradeCategory.Utility:
                ApplyMochila(Mathf.RoundToInt(value));
                break;

            default:
                Debug.LogWarning($"[Upgrades] Nenhum efeito mapeado para: {upgrade.upgradeName}");
                break;
        }
    }

    // ─────────────────────────────────────────────
    // EFEITOS INDIVIDUAIS
    // ─────────────────────────────────────────────

    private void ApplyVida(float novoMaximo)
    {
        if (playerLife == null) return;
        playerLife.SetMaxLife(novoMaximo);
    }

    private void ApplyOxigenio(float novoMaximo)
    {
        if (playerLife == null) return;
        playerLife.SetMaxOxygen(novoMaximo);
    }

    private void ApplyDano(float novoDano)
    {
        if (playerCombat == null) return;
        playerCombat.SetDamage(novoDano);
    }

    private void ApplyMochila(int novaCapacidade)
    {
        if (playerCollect == null) return;
        playerCollect.SetMaxCapacity(novaCapacidade);
    }
}