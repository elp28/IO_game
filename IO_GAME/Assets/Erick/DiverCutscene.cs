using UnityEngine;
using System.Collections;

public class DiverCutscene : MonoBehaviour
{
    [Header("Referências")]
    public GameObject player;
    public Animator playerAnimator;
    public MonoBehaviour playerMovementScript; // Arraste o script de movimentação aqui
    public GameObject bubblePrefab;
    public Transform bubbleSpawnPoint;
    public AudioSource sfxSource;

    [Header("Configurações")]
    public float moveDistance = 2f;
    public float moveSpeed = 1f;

    void Start()
    {
        StartCoroutine(ExecuteCutscene());
    }

    IEnumerator ExecuteCutscene()
    {
        // 1. Bloqueia o controle do jogador
        playerMovementScript.enabled = false;

        // 2. Inicia animação e áudio
        playerAnimator.SetBool("isSwimming", true);
        if (sfxSource) sfxSource.Play();

        // 3. Instancia as 5 bolhas
        for (int i = 0; i < 5; i++)
        {
            Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
        }

        // 4. Movimenta o player (2 metros para frente)
        Vector3 targetPosition = player.transform.position + player.transform.forward * moveDistance;
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.05f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null; // Espera o próximo frame
        }

        // 5. Finaliza: devolve o controle ao jogador
        playerMovementScript.enabled = true;
        // playerAnimator.SetBool("isSwimming", false); // Se quiser que pare de nadar após os 2m

        Debug.Log("Cutscene Finalizada!");
    }
}