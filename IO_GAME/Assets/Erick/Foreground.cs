using UnityEngine;

public class Foreground : MonoBehaviour
{
    [Header("Configurações de Parallax")]
    public Transform cameraTransform;
    [Range(-1f, 1f)]
    public float parallaxFactor = 0.2f;

    [Header("Configurações de Visibilidade")]
    public Transform player;
    public float detectionRadius = 2.0f;
    public float minAlpha = 0.3f;
    public float fadeSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startPos;
    private Vector3 startCamPos;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        startPos = transform.position;
        startCamPos = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 camDelta = cameraTransform.position - startCamPos;
        transform.position = startPos + camDelta * parallaxFactor;

        HandleAlphaFade();
    }

    void HandleAlphaFade()
    {
        // Distância direto do player ao objeto
        float distance = Vector2.Distance(player.position, transform.position);
        float targetAlpha = (distance < detectionRadius) ? minAlpha : 1.0f;

        Color curColor = spriteRenderer.color;
        float newAlpha = Mathf.Lerp(curColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
        spriteRenderer.color = new Color(curColor.r, curColor.g, curColor.b, newAlpha);
    }
}