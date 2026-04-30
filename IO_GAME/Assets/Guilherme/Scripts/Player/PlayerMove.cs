using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Joystick joy;
    Rigidbody2D rb;
    Vector2 movement;
    bool isMoving;
    Animator anim;
    SpriteRenderer sp;
    bool isLeft;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = joy.Horizontal;
        movement.y = joy.Vertical;

        if (movement != Vector2.zero)
        {
            sp.flipX = false;
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            isMoving = true;

            
            if (movement.x < 0)
            {
                isLeft = true;
                sp.flipY = true;
            }
            else
            {
                isLeft = false;
                sp.flipY = false;
            }
        }
        else
        {
            isMoving = false;
            sp.flipX = false; 

            
            if(isLeft)
            {
                rb.rotation = 180f; 
                sp.flipY = true;   
            }
            else
            {
                rb.rotation = 0f;   
                sp.flipY = false;
            }
        }
        
        anim.SetBool("isSwimming", isMoving);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}