using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;   

public class TrashItemGeneric : MonoBehaviour
{
    public enum TypeItem { glass, plastic, metal }
    
    [Header("Tipo de Recurso")]
    public TypeItem typeItem; 

    [Header("Efeito Flutuante (Bobbing)")]
    // Se esta curva estiver definida, ela controlará o movimento
    [SerializeField] private AnimationCurve curvaDeMovimento;
    
    // Se a curva estiver vazia, usamos os valores abaixo como padrão (Seno)
    [SerializeField] private float minAmplitude = 0.1f;
    [SerializeField] private float maxAmplitude = 0.3f;
    [SerializeField] private float minFrequencia = 1f;
    [SerializeField] private float maxFrequencia = 3f;
    
    // Variáveis que serão randomizadas no Start
    private float amplitudeAleatoria;
    private float frequenciaAleatoria;
    private float faseAleatoria; // Desincroniza os itens
    
    private Vector3 posicaoInicial;

    [SerializeField] float suctionSpeed = 10f; // Agora trataremos como velocidade de movimento
    private bool isBeingCollected = false;
    private Transform targetPlayer;
    private PlayerCollect bagReference;
   

    void Start()
    {
        // Salva a posição onde o item nasceu
        posicaoInicial = transform.position;

        // Randomiza os parâmetros de altura e velocidade dentro dos intervalos definidos
        amplitudeAleatoria = Random.Range(minAmplitude, maxAmplitude);
        frequenciaAleatoria = Random.Range(minFrequencia, maxFrequencia);
        
        // Randomiza o ponto de partida do ciclo (de 0 a 2PI radianos)
        faseAleatoria = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        if (isBeingCollected)
        {
            // Se o player ainda existir, move o lixo na direção da posição ATUAL do player
            if (targetPlayer != null)
            {
                // Move o objeto em direção ao player a cada frame (Perseguição)
                transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, suctionSpeed * Time.deltaTime);

                // Verifica se chegou perto o suficiente para "entrar" na mochila
                if (Vector3.Distance(transform.position, targetPlayer.position) < 0.1f)
                {
                    FinishCollection();
                }
            }
            return;
        }
        float novoY;

        // Se o desenvolvedor desenhou uma curva no Inspector, usamos ela
        if (curvaDeMovimento != null && curvaDeMovimento.length > 0)
        {
            // Avalia a curva baseada no tempo.
            // Para curvas padrão que vão de 0 a 1 no tempo, usamos Mathf.PingPong para loop infinito.
            float tempoCiclo = Mathf.PingPong(Time.time * frequenciaAleatoria + faseAleatoria, 1f);
            float valorDaCurva = curvaDeMovimento.Evaluate(tempoCiclo);
            
            // Aplica a amplitude aleatória ao resultado da curva
            novoY = valorDaCurva * amplitudeAleatoria;
        }
        else
        {
            // Se não houver curva, usamos o Seno padrão (vai de -1 a 1)
            novoY = Mathf.Sin(Time.time * frequenciaAleatoria + faseAleatoria) * amplitudeAleatoria;
        }

        // Aplica a nova posição mantendo o X e Z originais
        transform.position = posicaoInicial + new Vector3(0, novoY, 0);

      
    }
    public void GoToPlayer(Transform playerTransform, PlayerCollect bag)
    {
        if (isBeingCollected) return; 
        
        targetPlayer = playerTransform;
        bagReference = bag;
        isBeingCollected = true;

        // Efeito de "escala" com DOTween só para dar o Game Juice (opcional)
        //transform.DOScale(Vector3.one * 0.5f, 0.2f); 
    }

    private void FinishCollection()
    {
        bagReference.FinalizeCollection(this);
        // Usamos uma pequena animação de escala antes de destruir para ficar bonito
        transform.DOScale(Vector3.zero, 0.1f).OnComplete(() => Destroy(gameObject));
        isBeingCollected = false; // Evita múltiplas chamadas
    }
}

  
