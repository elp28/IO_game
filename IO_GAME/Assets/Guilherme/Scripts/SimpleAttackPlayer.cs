using UnityEngine;

public class SimpleAttackPlayer : MonoBehaviour
{
    CircleCollider2D colliderArea;
    [SerializeField] float numTrashsInBag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderArea = GetComponent<CircleCollider2D>();
        colliderArea.radius = transform.localScale.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        GenericEnemy[] trash = collision.gameObject.GetComponent<GenericEnemy[]>();
        foreach (GenericEnemy trashReal in trash)
        {
            trashReal.ItsOverForTrash();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GenericEnemy[] trash = collision.gameObject.GetComponent<GenericEnemy[]>();
        foreach (GenericEnemy trashReal in trash)
        {
            trashReal.ItsOverForTrash();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if(trash != null)
        {
            trash.ItsOverForTrash();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GenericEnemy trash = collision.gameObject.GetComponent<GenericEnemy>();
        if (trash != null)
        {
            trash.ItsOverForTrash();
        }
    }




}
