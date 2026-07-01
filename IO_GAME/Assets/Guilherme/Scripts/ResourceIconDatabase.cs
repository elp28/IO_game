using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Banco central de ícones de recursos.
/// Configure uma vez e todos os upgrades buscam automaticamente.
/// Crie via: Assets > Create > Shop > Resource Icon Database
/// </summary>
[CreateAssetMenu(fileName = "ResourceIconDatabase", menuName = "Shop/Resource Icon Database", order = 1)]
public class ResourceIconDatabase : ScriptableObject
{
    public static ResourceIconDatabase instance;

    [Serializable]
    public class ResourceIconEntry
    {
        public ResourceType resourceType;
        public Sprite icon;
    }

    [SerializeField] private List<ResourceIconEntry> entries = new List<ResourceIconEntry>();

    void OnEnable()
    {
        instance = this;
    }

    public Sprite GetIcon(ResourceType type)
    {
        foreach (var entry in entries)
        {
            if (entry.resourceType == type)
                return entry.icon;
        }

        Debug.LogWarning($"[ResourceIconDatabase] Ícone não encontrado para: {type}");
        return null;
    }
}