using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public List<GameObject> chunkPrefabs;
    public int poolSize = 6;
    public float moveSpeed = 8f;

    private readonly Queue<GameObject> active = new Queue<GameObject>();
    private Transform lastExit;
    public ObstacleFactory factory;
    public DifficultyController difficulty;

    void Start()
    {
        // spawn chunk แรกที่ตำแหน่ง origin
        SpawnFirstChunk();
        
        // spawn chunk ที่เหลือ
        for (int i = 1; i < poolSize; i++)
            SpawnNextChunk();
    }

    void Update()
    {
        MoveChunks();
        RecycleChunks();
    }

    void SpawnFirstChunk()
    {
        var prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.GetComponent<Chunk>().factory = factory;
        obj.GetComponent<Chunk>().difficulty = difficulty;
        active.Enqueue(obj);
        lastExit = obj.transform.Find("ExitPoint");
    }

    void SpawnNextChunk()
    {
        var prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        var obj = Instantiate(prefab, lastExit.position, Quaternion.identity);
        obj.GetComponent<Chunk>().factory = factory;
        obj.GetComponent<Chunk>().difficulty = difficulty;
        active.Enqueue(obj);
        lastExit = obj.transform.Find("ExitPoint");
    }

    void MoveChunks()
    {
        foreach (var chunk in active)
        {
            chunk.transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        }
    }

    void RecycleChunks()
    {
        var first = active.Peek();

        // เช็คว่า chunk หลุดกล้องหรือยัง
        if (first.transform.position.z < -50f)
        {
            active.Dequeue();
            Destroy(first);
            SpawnNextChunk();
        }
    }
}
