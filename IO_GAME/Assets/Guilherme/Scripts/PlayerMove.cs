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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
   void Update()
    {
        movement.x = joy.Horizontal;
        movement.y = joy.Vertical;
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            isMoving = true;
        }
        else
        {
            isMoving = false;
            rb.rotation = 0;
        }
        
        anim.SetBool("isSwimming", isMoving);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

    }
}
