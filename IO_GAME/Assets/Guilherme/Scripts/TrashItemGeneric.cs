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
    [SerializeField] private float jumpForce = 1.5f; 
    [SerializeField] private float jumpTime = 0.5f; 
    private bool isSpawning = true; 

    [Header("Efeito Flutuante (Bobbing)")]
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float minAmplitude = 0.1f;
    [SerializeField] private float maxAmplitude = 0.3f;
    [SerializeField] private float minFrequencie = 1f;
    [SerializeField] private float maxFrequencie = 3f;
    
    private float randomAmplitude;
    private float randomFrequencie;
    private float randomState;
    
    private Vector3 inicialPosition;

    [Header("Coleta")]
    [SerializeField] float suctionSpeed = 10f;
    private bool isBeingCollected = false;
    private Transform targetPlayer;
    private PlayerCollect bagReference;

    void Start()
    {
        Vector3 originalScale = transform.localScale;

        randomAmplitude = Random.Range(minAmplitude, maxAmplitude);
        randomFrequencie = Random.Range(minFrequencie, maxFrequencie);
        randomState = Random.Range(0f, Mathf.PI * 2f);

        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale, jumpTime).SetEase(Ease.OutBack);

        transform.DOJump(transform.position, jumpForce, 1, jumpTime).SetEase(Ease.OutQuad).OnComplete(() => {inicialPosition = transform.position; isSpawning = false; });
    }

    void Update()
    {
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

        float newY;

        if (movementCurve != null && movementCurve.length > 0)
        {
            float cicleTime = Mathf.PingPong(Time.time * randomFrequencie + randomState, 1f);
            float curveValue = movementCurve.Evaluate(cicleTime);
            newY = curveValue * randomAmplitude;
        }
        else
        {
            newY = Mathf.Sin(Time.time * randomFrequencie + randomState) * randomAmplitude;
        }
        transform.position = inicialPosition + new Vector3(0, newY, 0);
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