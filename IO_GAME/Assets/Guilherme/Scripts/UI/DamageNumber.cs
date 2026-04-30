using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshPro tmp; 
    [SerializeField] private float height;
    [SerializeField] private float speed;

    public void Init(float damage)
    {
        tmp.text = Mathf.RoundToInt(damage).ToString();

        
        transform.DOMove(transform.position + Vector3.up * height, speed)
            .SetEase(Ease.OutCubic);

        tmp.DOFade(0f, 0.8f)
            .SetEase(Ease.InQuad)
            .OnComplete(() => Destroy(gameObject));
    }
}