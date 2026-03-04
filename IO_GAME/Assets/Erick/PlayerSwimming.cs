using UnityEngine;

public class PlayerSwimming : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float swimSpeed = 5f;        // Velocidade horizontal
    public float swimForce = 6f;        // Força do impulso para cima
    public float waterDrag = 2f;        // Resistência da água (ajude a parar o player)

    [Header("Referências")]
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sprite;

    private Vector2 moveInput;

    void Update()
    {
        // 1. Captura de Inputs
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. Lógica de Animação e Direção
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // 3. Aplicação do Movimento
        ApplySwimmingPhysics();
    }

    void ApplySwimmingPhysics()
    {
        // Aplicamos uma velocidade baseada no input
        // O drag (arrasto) do Rigidbody deve ser alto para ele não "deslizar" eternamente
        rb.linearVelocity = new Vector2(moveInput.x * swimSpeed, moveInput.y * swimForce);

        // Se quiser um efeito mais fluido, você pode usar AddForce em vez de Velocity
        // rb.AddForce(moveInput * swimSpeed);
    }

    void UpdateAnimations()
    {
        // Verifica se o player está se movendo para ativar a animação
        bool isMoving = moveInput.magnitude > 0;
        anim.SetBool("isSwimming", isMoving);

        // Inverte o sprite para o lado que está nadando
        if (moveInput.x > 0) sprite.flipX = false;
        else if (moveInput.x < 0) sprite.flipX = true;
    }
}