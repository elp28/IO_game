using UnityEngine;
using UnityEngine.InputSystem; // Importante adicionar essa linha!

public partial class PlayerSwimming : MonoBehaviour
{
    [Header("Configurações")]
    public float swimSpeed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sprite;

    private Vector2 moveInput;

    // Esse método é chamado automaticamente pelo componente "Player Input"
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * swimSpeed;
    }

    void Update()
    {
        // Animação e Direção
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        anim.SetBool("isSwimming", isMoving);

        if (moveInput.x > 0) sprite.flipX = false;
        else if (moveInput.x < 0) sprite.flipX = true;
    }
}