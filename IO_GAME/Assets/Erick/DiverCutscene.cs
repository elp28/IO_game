using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiverCutscene : MonoBehaviour
{
    [Header("Referências do Player")]
    public GameObject player;
    public Animator playerAnimator;
    public MonoBehaviour playerMovementScript;
    public Transform targetPoint;

    [Header("Referências da UI")]
    public Image fadePanel;
    public CanvasGroup textoGrupo1;   // Seus textos precisam desse componente!
    public CanvasGroup textoGrupo2;

    [Header("Efeitos & Sons")]
    public GameObject bubblePrefab;
    public Transform bubbleSpawnPoint;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Configurações de Tempo")]
    public float tempoPretoSolido = 5.0f;
    public float duracaoFadeTextos = 1.5f;
    public float duracaoFadePainel = 2.0f;
    public float moveSpeed = 1.5f;

    void Start()
    {
        // Garante o estado inicial
        if (fadePanel != null)
        {
            fadePanel.gameObject.SetActive(true);
            fadePanel.color = new Color(0, 0, 0, 1);
        }
        if (textoGrupo1 != null) textoGrupo1.alpha = 1f;
        if (textoGrupo2 != null) textoGrupo2.alpha = 1f;

        StartCoroutine(ExecuteCutsceneSequence());
    }

    IEnumerator ExecuteCutsceneSequence()
    {
        // TRAVA O PLAYER
        playerMovementScript.enabled = false;
        playerAnimator.SetBool("isSwimming", true);

        // 1. ESPERA COM TEXTO NA TELA
        yield return new WaitForSeconds(tempoPretoSolido);

        // 2. FADE DOS TEXTOS (SUMINDO)
        float timer = 0f;
        while (timer < duracaoFadeTextos)
        {
            timer += Time.deltaTime;
            float alphaValue = Mathf.Lerp(1f, 0f, timer / duracaoFadeTextos);
            if (textoGrupo1) textoGrupo1.alpha = alphaValue;
            if (textoGrupo2) textoGrupo2.alpha = alphaValue;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        // 3. FADE DO PAINEL PRETO (REVELANDO O JOGO)
        timer = 0f;
        while (timer < duracaoFadePainel)
        {
            timer += Time.deltaTime;
            if (fadePanel != null)
            {
                Color c = fadePanel.color;
                c.a = Mathf.Lerp(1f, 0f, timer / duracaoFadePainel);
                fadePanel.color = c;
            }
            yield return null;
        }
        if (fadePanel != null) fadePanel.gameObject.SetActive(false);

        // 4. SOM + MÚSICA + BOLHAS (TUDO JUNTO AGORA)
        if (sfxSource) sfxSource.Play();
        if (musicSource) musicSource.Play();

        for (int i = 0; i < 5; i++)
        {
            if (bubblePrefab && bubbleSpawnPoint)
                Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
        }

        // 5. MOVIMENTAÇÃO
        float direcaoX = targetPoint.position.x - player.transform.position.x;
        player.transform.eulerAngles = new Vector3(0, direcaoX > 0 ? 0 : 180, 0);

        while (Vector3.Distance(player.transform.position, targetPoint.position) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        player.transform.position = targetPoint.position;

        // 6. FINALIZAÇÃO
        playerAnimator.SetBool("isSwimming", false);
        playerAnimator.SetTrigger("standIdle");
        yield return new WaitForSeconds(0.5f);
        playerMovementScript.enabled = true;
    }
}