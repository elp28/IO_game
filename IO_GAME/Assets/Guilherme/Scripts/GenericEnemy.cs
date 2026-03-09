using UnityEngine;
using System.Collections;

public class GenericEnemy : MonoBehaviour
{
    public float speed;
    public BoxCollider2D fisCollider;
    public CircleCollider2D areaCollider;
    public Rigidbody2D rb;
    public bool feltPlayer;
    public  float life;
    public  float damage;
    public PlayerSwimming player;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = GetComponent<PlayerSwimming>();
        if (player != null)
        {
            feltPlayer = true;
        }
    }

    public void PlayerClosed()
    {

    }


}
