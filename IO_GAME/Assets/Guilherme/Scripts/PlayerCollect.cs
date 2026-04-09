using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    CircleCollider2D colliderArea;
    int numTrashInBag;
    [SerializeField] int maxTrashInBag;
    SpriteRenderer sp;
    bool bagIsFull;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderArea = GetComponent<CircleCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       if(numTrashInBag >=  maxTrashInBag)
       {
           bagIsFull = true;
           DesactiveCollector();
       }
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null && !bagIsFull)
        {
            trash.ItsOverForTrash(false, this);
        }

        BoxCollect collector = collision.gameObject.GetComponent<BoxCollect>();
        if(collector != null && numTrashInBag > 0)
        {
            collector.CollectFromPlayer(numTrashInBag);
            ActiveCollected();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null && !bagIsFull)
        {
            trash.ItsOverForTrash(true, this);

        }
    }

    public void PickedTrash()
    {
        if (!bagIsFull)
        {
            print("pegou o dog");
            numTrashInBag++;
        }

    }

    void DesactiveCollector()
    { 
        sp.enabled = false;
    }

    void ActiveCollected()
    {
        sp.enabled = true;
        numTrashInBag = 0;
        bagIsFull = false;
    }

}
