using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;   

public class TrashItemGeneric : MonoBehaviour
{
    public enum TypeItem { glass, plastic, metal }
    
    [Header("Tipo de Recurso")]
    public TypeItem typeItem; 

    [Header("Animação de Spawn")]
    [SerializeField] private float forcaDoPulo = 1.5f; // Altura que ele vai subir ao nascer
    [SerializeField] private float tempoDoPulo = 0.5f; // Quanto tempo dura o pulo até cair
    private bool isSpawning = true; // Trava o Update enquanto estiver na animação inicial

    [Header("Efeito Flutuante (Bobbing)")]
    [SerializeField] private AnimationCurve curvaDeMovimento;
    [SerializeField] private float minAmplitude = 0.1f;
    [SerializeField] private float maxAmplitude = 0.3f;
    [SerializeField] private float minFrequencia = 1f;
    [SerializeField] private float maxFrequencia = 3f;
    
    private float amplitudeAleatoria;
    private float frequenciaAleatoria;
    private float faseAleatoria;
    
    private Vector3 posicaoInicial;

    [Header("Coleta")]
    [SerializeField] float suctionSpeed = 10f;
    private bool isBeingCollected = false;
    private Transform targetPlayer;
    private PlayerCollect bagReference;

    void Start()
    {
        // 1. Salva o tamanho original que você definiu no Unity/Inspector
        Vector3 escalaOriginal = transform.localScale;

        // Randomiza os parâmetros do Bobbing
        amplitudeAleatoria = Random.Range(minAmplitude, maxAmplitude);
        frequenciaAleatoria = Random.Range(minFrequencia, maxFrequencia);
        faseAleatoria = Random.Range(0f, Mathf.PI * 2f);

        // 2. Começa do zero e cresce até a ESCALA ORIGINAL (em vez de Vector3.one)
        transform.localScale = Vector3.zero;
        transform.DOScale(escalaOriginal, tempoDoPulo).SetEase(Ease.OutBack);

        // Faz o pulo
        transform.DOJump(transform.position, forcaDoPulo, 1, tempoDoPulo).SetEase(Ease.OutQuad).OnComplete(() => {posicaoInicial = transform.position; isSpawning = false; });
    }

    void Update()
    {
        // Se estiver na animação de nascer, não faz o bobbing ainda!
        if (isSpawning) return;

        if (isBeingCollected)
        {
            if (targetPlayer != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, suctionSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPlayer.position) < 0.1f)
                {
                    FinishCollection();
                }
            }
            return;
        }

        float novoY;

        if (curvaDeMovimento != null && curvaDeMovimento.length > 0)
        {
            float tempoCiclo = Mathf.PingPong(Time.time * frequenciaAleatoria + faseAleatoria, 1f);
            float valorDaCurva = curvaDeMovimento.Evaluate(tempoCiclo);
            novoY = valorDaCurva * amplitudeAleatoria;
        }
        else
        {
            novoY = Mathf.Sin(Time.time * frequenciaAleatoria + faseAleatoria) * amplitudeAleatoria;
        }

        // Aplica a nova posição mantendo o X e Z originais (que foram definidos após o pulo)
        transform.position = posicaoInicial + new Vector3(0, novoY, 0);
    }

    public void GoToPlayer(Transform playerTransform, PlayerCollect bag)
    {
        if (isBeingCollected) return; 
        
        targetPlayer = playerTransform;
        bagReference = bag;
        isBeingCollected = true;
    }

    private void FinishCollection()
    {
        bagReference.FinalizeCollection(this);
        transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => Destroy(gameObject));
        isBeingCollected = false; 
    }
}