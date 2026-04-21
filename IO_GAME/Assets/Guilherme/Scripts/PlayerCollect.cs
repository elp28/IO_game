using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int numTrashInBag;
    [SerializeField] int maxTrashInBag = 10;
    SpriteRenderer sp;
    bool bagIsFull;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Lógica para coletar o lixo solto no chão (precisa ter a tag "Trash")
        if (collision.CompareTag("Trash") && !bagIsFull)
        {
            numTrashInBag++;
            Destroy(collision.gameObject); // Destrói o item do chão
            print("Lixo coletado: " + numTrashInBag + "/" + maxTrashInBag);

            if (numTrashInBag >= maxTrashInBag)
            {
                bagIsFull = true;
                print("Mochila Cheia! Volte para a base.");
            }
        }

        // 2. Lógica para descarregar na Estação
        BoxCollect station = collision.gameObject.GetComponent<BoxCollect>();
        if (station != null && numTrashInBag > 0)
        {
            station.CollectFromPlayer(numTrashInBag);
            ClearBag();
        }
    }

    // Usado pela estação ou quando o jogador morre (para perder os itens)
    public void ClearBag()
    {
        numTrashInBag = 0;
        bagIsFull = false;
        print("Mochila esvaziada.");
    }
}