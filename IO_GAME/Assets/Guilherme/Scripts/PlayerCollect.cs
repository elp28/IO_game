using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    CircleCollider2D colliderArea;
    int numTrashInBag;
    [SerializeField] int maxTrashInBag;
    SpriteRenderer sp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderArea = GetComponent<CircleCollider2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(numTrashInBag < maxTrashInBag)
        {
            colliderArea.enabled = true;
            sp.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<GenericEnemy>() || collision.GetComponent<BoxCollect>())return;

        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null)
        {
            trash.ItsOverForTrash(false, this);
        }

        BoxCollect collector = collision.gameObject.GetComponent<BoxCollect>();
        if(collector != null && numTrashInBag > 0)
        {
            collector.CollectFromPlayer(numTrashInBag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null)
        {
            trash.ItsOverForTrash(true, this);

        }
    }

    public void PickedTrash()
    {
        if (numTrashInBag < maxTrashInBag)
            numTrashInBag++;
        else
        {
            DesactiveThisAttack();
        }

    }

    void DesactiveThisAttack()
    {
        colliderArea.enabled = false;
        sp.enabled = false;
    }

}
