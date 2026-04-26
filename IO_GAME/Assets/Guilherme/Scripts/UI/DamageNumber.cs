using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] private TextMeshPro tmp; // TextMeshPro (não UI, Pro normal)

    public void Init(float damage)
    {
        tmp.text = Mathf.RoundToInt(damage).ToString();

        // Sobe e some
        transform.DOMove(transform.position + Vector3.up * 1.5f, 0.8f)
            .SetEase(Ease.OutCubic);

        tmp.DOFade(0f, 0.8f)
            .SetEase(Ease.InQuad)
            .OnComplete(() => Destroy(gameObject));
    }
}