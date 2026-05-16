using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representa uma oferta gerada em runtime quando a loja abre.
/// Não altera nenhum dado do UpgradeData — apenas lê e calcula.
/// Descartada quando a loja fecha.
/// </summary>
public class ShopOffer
{
    // ─────────────────────────────────────────────
    // DADOS DA OFERTA
    // ─────────────────────────────────────────────

    /// <summary>O upgrade sorteado. Fonte de verdade dos dados base.</summary>
    public readonly UpgradeData Upgrade;

    /// <summary>Nível atual do jogador nesse upgrade (0 = não comprado).</summary>
    public readonly int CurrentLevel;

    /// <summary>Nível que será atingido se a oferta for comprada.</summary>
    public readonly int NextLevel;

    /// <summary>Valor do efeito após a compra (ex: 30 HP, 8 slots).</summary>
    public readonly float ValueAfterPurchase;

    /// <summary>Custo calculado e escalonado para o próximo nível.</summary>
    public readonly List<ResourceCost> Cost;

    // ─────────────────────────────────────────────
    // DADOS VISUAIS (prontos para a UI consumir)
    // ─────────────────────────────────────────────

    /// <summary>Nome do upgrade.</summary>
    public string DisplayName  => Upgrade.upgradeName;

    /// <summary>Descrição do upgrade.</summary>
    public string Description  => Upgrade.description;

    /// <summary>Ícone do upgrade.</summary>
    public Sprite Icon         => Upgrade.icon;

    /// <summary>True se o jogador já está no nível máximo desse upgrade.</summary>
    public bool IsMaxLevel     => Upgrade.IsMaxLevel(CurrentLevel);

    // ─────────────────────────────────────────────
    // CONSTRUTOR
    // Criado pelo ShopManager ao gerar as ofertas.
    // ─────────────────────────────────────────────

    /// <summary>
    /// Cria uma oferta a partir de um upgrade e do nível atual do jogador.
    /// Todos os valores são calculados aqui e ficam imutáveis.
    /// </summary>
    public ShopOffer(UpgradeData upgrade, int currentLevel)
    {
        Upgrade      = upgrade;
        CurrentLevel = currentLevel;
        NextLevel    = currentLevel + 1;

        ValueAfterPurchase = upgrade.GetValueAtLevel(NextLevel);
        Cost               = upgrade.GetCostsAtLevel(NextLevel);
    }
}