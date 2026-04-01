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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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
            rb.rotation = 0;
            if(isLeft)
                sp.flipX = true;
            sp.flipY = false;
        }
        
        anim.SetBool("isSwimming", isMoving);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

    }
}
