using UnityEngine;

public class SimpleAttackPlayer : MonoBehaviour
{
    CircleCollider2D colliderArea;
    [SerializeField] float numTrashsInBag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderArea = GetComponent<CircleCollider2D>();
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
            print(trash);
            trash.ItsOverForTrash(false);
        }
    }   

    private void OnTriggerExit2D(Collider2D collision)
    {
        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null)
        {
            trash.ItsOverForTrash(true);
        }
    }




}
