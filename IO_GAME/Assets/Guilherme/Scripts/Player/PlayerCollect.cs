using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private int maxCapacity = 10;
    [Tooltip("Tempo entre cada item sendo entregue na estação.")]
    [SerializeField] private float deliveryInterval = 0.15f;

    [Header("Managers")]
    [SerializeField] private HUDManager hudManager;

    [Header("Ícones Visuais")]
    [SerializeField] private GameObject resourceIconPrefab;
    [SerializeField] private Sprite iconGlass;
    [SerializeField] private Sprite iconPlastic;
    [SerializeField] private Sprite iconMetal;

    private int glassCount, plasticCount, metalCount;
    private int currentTotal;
    public int CurrentTotal => currentTotal;
    public int MaxCapacity  => maxCapacity;

    private List<TrashItemGeneric.TypeItem> _itemTypes = new List<TrashItemGeneric.TypeItem>();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coletable") && currentTotal < maxCapacity)
        {
            TrashItemGeneric trash = collision.GetComponent<TrashItemGeneric>();
            if (trash != null)
            {
                switch (trash.typeItem)
                {
                    case TrashItemGeneric.TypeItem.glass:   glassCount++;   break;
                    case TrashItemGeneric.TypeItem.plastic: plasticCount++; break;
                    case TrashItemGeneric.TypeItem.metal:   metalCount++;   break;
                }

                currentTotal++;
                _itemTypes.Add(trash.typeItem);
                trash.GoToPlayer(this.transform, this);
            }
        }

        BoxCollect boxCollect = collision.gameObject.GetComponent<BoxCollect>();
        if (boxCollect != null)
        {
            hudManager.ActiveButtonStation(true);
            hudManager.SetCurrentStation(boxCollect.ShopStation);
            StartCoroutine(DeliverItemsOneByOne(boxCollect.transform));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BoxCollect>())
        {
            hudManager.ActiveButtonStation(false);
            hudManager.ClearCurrentStation();
        }
    }

    private IEnumerator DeliverItemsOneByOne(Transform station)
    {
        if (currentTotal <= 0) yield break;

        List<TrashItemGeneric.TypeItem> typesToDeliver = new List<TrashItemGeneric.TypeItem>(_itemTypes);
        _itemTypes.Clear();

        // Não zera currentTotal aqui — vai descendo 1 por 1
        glassCount   = 0;
        plasticCount = 0;
        metalCount   = 0;

        foreach (var type in typesToDeliver)
        {
            SpawnResourceIcon(type, station);

            yield return new WaitForSeconds(deliveryInterval);
        }
    }

    private void SpawnResourceIcon(TrashItemGeneric.TypeItem type, Transform station)
    {
        if (resourceIconPrefab == null) return;

        GameObject obj = Instantiate(resourceIconPrefab, transform.position, Quaternion.identity);
        ResourceIconVisual icon = obj.GetComponent<ResourceIconVisual>();
        if (icon == null) return;

        Sprite sprite = type switch
        {
            TrashItemGeneric.TypeItem.glass   => iconGlass,
            TrashItemGeneric.TypeItem.plastic => iconPlastic,
            TrashItemGeneric.TypeItem.metal   => iconMetal,
            _                                 => null
        };

        TrashItemGeneric.TypeItem capturedType = type;

        icon.Setup(sprite, station, () =>
        {
            // Converte o recurso ao chegar
            switch (capturedType)
            {
                case TrashItemGeneric.TypeItem.glass:
                    ResourceManager.instance?.ConvertTrashToResource(1, 0, 0);
                    break;
                case TrashItemGeneric.TypeItem.plastic:
                    ResourceManager.instance?.ConvertTrashToResource(0, 1, 0);
                    break;
                case TrashItemGeneric.TypeItem.metal:
                    ResourceManager.instance?.ConvertTrashToResource(0, 0, 1);
                    break;
            }

            // Desce o contador da mochila 1 por vez
            currentTotal = Mathf.Max(0, currentTotal - 1);
        });
    }

    public void ClearBag()
    {
        glassCount   = 0;
        plasticCount = 0;
        metalCount   = 0;
        currentTotal = 0;
        _itemTypes.Clear();
    }

    public void SetMaxCapacity(int newCapacity)
    {
        maxCapacity = newCapacity;
    }
}