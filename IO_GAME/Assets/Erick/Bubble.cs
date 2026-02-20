using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f; // Ajuste para o tempo da sua animação

    void Start()
    {
        // Destrói a bolha após o tempo de vida definido
        Destroy(gameObject, lifeTime);
    }
}