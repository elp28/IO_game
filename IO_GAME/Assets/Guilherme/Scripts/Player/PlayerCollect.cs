using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private int maxCapacity = 10;
    
    // Contagem temporária (o que está na mochila agora)
    private int glassCount, plasticCount, metalCount;
    private int currentTotal;
    TrashItemGeneric trash;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("coletable") && currentTotal < maxCapacity)
        {
            trash = collision.GetComponent<TrashItemGeneric>();
            if (trash != null)
            {
                trash.GoToPlayer(transform);  
                AddToBag(trash.typeItem);   
                Destroy(collision.gameObject);
            }
        }

      
        if (collision.gameObject.GetComponent<BoxCollect>()) 
        {
            DeliverTrash();
        }
    }

    private void AddToBag(TrashItemGeneric.TypeItem type)
    {
        switch (type)
        {
            case TrashItemGeneric.TypeItem.glass: glassCount++; break;
            case TrashItemGeneric.TypeItem.plastic: plasticCount++; break;
            case TrashItemGeneric.TypeItem.metal: metalCount++; break;
        }
        currentTotal++;
        Debug.Log($"Mochila: {currentTotal}/{maxCapacity} Itens");
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