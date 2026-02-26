using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("config")]
    public GameObject bubblePrefab;
    public float spawnRate = 1.5f; 

    [Header("spawner")]
    public Vector2 minSpawnPos;
    public Vector2 maxSpawnPos;

    void Start()
    {
        
        InvokeRepeating(nameof(SpawnBubble), 0.5f, spawnRate);
    }

    void SpawnBubble()
    {
        
        Vector3 randomPos = new Vector3(
            Random.Range(minSpawnPos.x, maxSpawnPos.x),
            Random.Range(minSpawnPos.y, maxSpawnPos.y),
            
            0 
        );

        
        Instantiate(bubblePrefab, randomPos, Quaternion.identity, transform);
    }
}