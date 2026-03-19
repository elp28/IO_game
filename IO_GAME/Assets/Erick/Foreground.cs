using UnityEngine;
using UnityEngine.Tilemaps;

public class Foreground : MonoBehaviour
{
    [Header("Configurações de Parallax")]
    public Transform cameraTransform;
    public float parallaxFactor = -0.5f; // Valor negativo faz ele vir "na frente"

    [Header("Configurações de Visibilidade")]
    public Transform player;
    public float detectionRadius = 2.0f;
    public float minAlpha = 0.3f; // Transparência quando o player está atrás
    public float fadeSpeed = 5f;

    private Tilemap tilemap;
    private Vector3 lastCameraPosition;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // 1. Efeito de Parallax (Movimento)
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += deltaMovement * parallaxFactor;
        lastCameraPosition = cameraTransform.position;

        // 2. Game Juice: Fade Alpha se o Player estiver perto/atrás
        HandleAlphaFade();
    }

    void HandleAlphaFade()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        float targetAlpha = (distance < detectionRadius) ? minAlpha : 1.0f;

        Color curColor = tilemap.color;
        float newAlpha = Mathf.MoveTowards(curColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
        tilemap.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
}