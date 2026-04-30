using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private int maxCapacity = 10;
    
    private int glassCount, plasticCount, metalCount;
    private int currentTotal;
    public int CurrentTotal => currentTotal;
    public int MaxCapacity => maxCapacity;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coletable") && currentTotal < maxCapacity)
        {
            TrashItemGeneric trash = collision.GetComponent<TrashItemGeneric>();
            if (trash != null)
            {
                trash.GoToPlayer(this.transform, this);
                
                currentTotal++; 
            }
        }

        if (collision.gameObject.GetComponent<BoxCollect>()) 
        {
            DeliverTrash();
        }
    }

    public void FinalizeCollection(TrashItemGeneric trash)
    {
        switch (trash.typeItem)
        {
            case TrashItemGeneric.TypeItem.glass: glassCount++; break;
            case TrashItemGeneric.TypeItem.plastic: plasticCount++; break;
            case TrashItemGeneric.TypeItem.metal: metalCount++; break;
        }
        
        Debug.Log($"Item processado! Total na mochila: {currentTotal}");
    }

    private void DeliverTrash()
    {
        if (currentTotal <= 0) return;

        
        if (ResourceManager.instance != null)
        {
            ResourceManager.instance.ConvertTrashToResource(glassCount, plasticCount, metalCount);
            
            
            ClearBag();
            Debug.Log("Lixo descarregado e transformado em recurso!");
        }
    }

    public void ClearBag()
    {
        glassCount = 0;
        plasticCount = 0;
        metalCount = 0;
        currentTotal = 0;
    }

 
}