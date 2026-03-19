using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Sound.Instance != null)  // Agora funciona!
            Sound.Instance.PlayClick();
    }
}