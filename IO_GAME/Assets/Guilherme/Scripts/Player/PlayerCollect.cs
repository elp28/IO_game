using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private int maxCapacity = 10;

    [Header("Managers")]
    [SerializeField] HUDManager hudManager;
    
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
        
                switch (trash.typeItem)
                {
                    case TrashItemGeneric.TypeItem.glass:   glassCount++;   break;
                    case TrashItemGeneric.TypeItem.plastic: plasticCount++; break;
                    case TrashItemGeneric.TypeItem.metal:   metalCount++;   break;
                }

                currentTotal++;
                trash.GoToPlayer(this.transform, this);
            }
        }

        if (collision.gameObject.GetComponent<BoxCollect>()) 
        {
            DeliverTrash();
            print("oiCheguei");
            hudManager.ActiveButtonStation(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<BoxCollect>())
        {
            hudManager.ActiveButtonStation(false);
        }
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