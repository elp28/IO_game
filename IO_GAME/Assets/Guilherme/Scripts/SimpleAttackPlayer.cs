using UnityEngine;

public class SimpleAttackPlayer : MonoBehaviour
{
    CircleCollider2D colliderArea;
    float numTrashInBag;
    [SerializeField] float maxTrashInBag;
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
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    
        if (collision.isTrigger) return; 

        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if(trash != null)
        {
            trash.ItsOverForTrash(false, this);
        }
    }   

    private void OnTriggerExit2D(Collider2D collision)
    {
        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null)
        {
            trash.ItsOverForTrash(true, this );
            
        }
    }

    public void PickedTrash()
    {
        if(numTrashInBag < maxTrashInBag)
        numTrashInBag ++;
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
