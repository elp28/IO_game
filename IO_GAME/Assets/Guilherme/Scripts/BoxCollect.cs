using UnityEngine;

public class BoxCollect : MonoBehaviour
{
    CircleCollider2D areaCollider;
    int trashCollected;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        areaCollider = GetComponent<CircleCollider2D>();
        trashCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CollectFromPlayer(int trashOfPlayer)
    {
        trashCollected += trashOfPlayer;
        print(trashCollected);
    }

    
}
