using UnityEngine;
using System.Collections;

public class GenericEnemy : MonoBehaviour
{
    public float speed;
    public BoxCollider2D fisCollider;
    public CircleCollider2D areaCollider;
    public Rigidbody2D rb;
    bool feltPlayer;
    public bool FeltPlayer => feltPlayer;
    public  float life;
    public  float damage;
    public PlayerSwimming player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<PlayerSwimming>();
        if (player != null)
        {
            feltPlayer = true;
        }
    }

    public void PlayerClosed()
    {

    }

    


}
