using UnityEngine;
using DG.Tweening;

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

    [SerializeField] float suctionSpeed = 0.5f;
   

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

    public void GoToPlayer(Transform posPlayer)
    {
        transform.DOMove(posPlayer.position, suctionSpeed);
        
    }

  
}