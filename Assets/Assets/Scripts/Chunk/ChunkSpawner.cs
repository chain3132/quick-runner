using System.Collections.Generic;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public List<GameObject> chunkPrefabs;
    public List<GameObject> specialPrefabs;

    public int poolSize = 6;
    public float moveSpeed = 8f;

    private readonly Queue<GameObject> active = new Queue<GameObject>();
    private Transform lastExit;
    public ObstacleFactory factory;
    public DifficultyController difficulty;
    private int chunkCount = 0;
    private bool previousWasLongGap = false;
    private int longGapRemaining = 0;


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
    SpecialType RollSpecialTypeBalanced()
    {
        float r = Random.value;
        if (r < 0.4f) return SpecialType.BreakWall;
        else if (r < 0.8f) return SpecialType.ShrinkTunnel;
        else return SpecialType.LongGap; // 20%
    }
    void SpawnFirstChunk()
    {
        var prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.GetComponent<Chunk>().factory = factory;
        obj.GetComponent<Chunk>().difficulty = difficulty;
        obj.GetComponent<Chunk>().isFirstChunk = true;
        active.Enqueue(obj);
        lastExit = obj.transform.Find("ExitPoint");
    }

    void SpawnNextChunk()
    {
            chunkCount++;

            // ถ้ายังอยู่ในช่วง LongGap ก็ skip
            if (longGapRemaining > 0)
            {
                longGapRemaining--;
                AdvanceExit();
                return;
            }

            bool spawnSpecial = false;

            if (chunkCount > 3) // unlock หลัง 3 chunk
            {
                if (difficulty.ShouldSpawnSpecial())
                {
                    spawnSpecial = true;
                    Debug.Log("Spawn Special Chunk");
                }
                    
            }

            SpecialType? special = null;

            if (spawnSpecial)
                special = RollSpecialTypeBalanced();

            // ถ้า roll ได้ LongGap
            if (special == SpecialType.LongGap && !previousWasLongGap)
            {
                previousWasLongGap = true;
                longGapRemaining = Random.Range(1, 3); 
                AdvanceExit();
                return;
            }

            // ถ้าเป็น special อื่น
            previousWasLongGap = false;
            if (special != null)
            {
                SpawnSpecialChunk(special.Value);
            }
            else{ SpawnNormalChunk(); }
                
            
        }
    void SpawnSpecialChunk(SpecialType t)
    {
        GameObject prefab = null;

        switch(t)
        {
            case SpecialType.BreakWall:
                prefab = specialPrefabs[0];
                break;
            case SpecialType.ShrinkTunnel:
                prefab = specialPrefabs[1];
                break;
        }

        var obj = Instantiate(prefab, lastExit.position, Quaternion.identity);

        var c = obj.GetComponent<Chunk>();
        c.factory = factory;
        c.difficulty = difficulty;
        c.SetSpecial(t);

        active.Enqueue(obj);
        lastExit = obj.transform.Find("ExitPoint");
    }
    void SpawnNormalChunk()
    {
        var prefab = chunkPrefabs[0];
        var obj = Instantiate(prefab, lastExit.position, Quaternion.identity);

        var c = obj.GetComponent<Chunk>();
        c.factory = factory;
        c.difficulty = difficulty;
        c.SetNormal();

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
    void AdvanceExit()
    {
        float chunkLength = 20f; // ตาม prefab
        lastExit.position += Vector3.forward * chunkLength;
    }



}
