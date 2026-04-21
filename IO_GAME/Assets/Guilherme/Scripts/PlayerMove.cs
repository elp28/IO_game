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

            // Evita que o peixe/personagem fique de ponta cabeça ao nadar para a esquerda
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
            sp.flipX = false; // Não usamos mais o flipX para simular que virou

            // CORREÇÃO: Mantemos a rotação física para a direção correta
            if(isLeft)
            {
                rb.rotation = 180f; // Rotaciona o corpo todo para a esquerda
                sp.flipY = true;    // Mantém o sprite em pé
            }
            else
            {
                rb.rotation = 0f;   // Mantém o corpo para a direita
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