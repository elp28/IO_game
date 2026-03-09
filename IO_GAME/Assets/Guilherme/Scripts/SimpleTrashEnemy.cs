using UnityEngine;

public class SimpleTrashEnemy : GenericEnemy
{
    GenericEnemy genericEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        genericEnemy = GetComponent<GenericEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(genericEnemy.feltPlayer)
        {
            print("oi");
            rb.MovePosition(Vector2.MoveTowards(transform.localPosition, new Vector2(player.transform.position.x, player.transform.position.y), speed * Time.deltaTime));
        }
    }
}
