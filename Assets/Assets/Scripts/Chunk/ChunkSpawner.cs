using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    
    public List<GameObject> chunkPrefabs;
    public List<GameObject> specialPrefabs;
    public GameObject longGapPrefab;
    public int poolSize = 6;
    
    private readonly Queue<GameObject> active = new Queue<GameObject>();
    private Transform lastExit;
    public ObstacleFactory factory;
    public DifficultyController difficulty;
    private int chunkCount = 0;
    private bool previousWasLongGap = false;
    private int longGapRemaining = 0;
    private bool lastWasSpecial = false;
    private float currentSpeed;
    
    [SerializeField] bool testMode = false;
    [SerializeField] SpecialType testSpecialType = SpecialType.ShrinkTunnel;

    void Start()
    {
        currentSpeed = DistanceManager.Instance.speed;
        // spawn chunk แรกที่ตำแหน่ง origin
        SpawnFirstChunk();
        
        // spawn chunk ที่เหลือ
        for (int i = 1; i < poolSize; i++)
            SpawnNextChunk();
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }

        if (currentSpeed != DistanceManager.Instance.speed)
        {
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                DistanceManager.Instance.speed,
                2f * Time.deltaTime);
        }
        MoveChunks();
        RecycleChunks();
    }
    SpecialType RollSpecialTypeBalanced()
    {
        if (testMode)
        {
            return testSpecialType;
        }
        float r = Random.value;
        if (r < 0.5f) return SpecialType.BreakWall;
        else if (r < 0.8f) return SpecialType.ShrinkTunnel;
        else return SpecialType.LongGap; // 20%

    }
    void SpawnFirstChunk()
    {
        var prefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Count)];
        var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.transform.position = new Vector3(0, -4.75f, 0);
        var chuck = obj.GetComponent<Chunk>();
        chuck.factory = factory;
        chuck.difficulty = difficulty;
        chuck.isFirstChunk = true;
        active.Enqueue(obj);
        lastExit = obj.transform.Find("ExitPoint");
    }

    void SpawnNextChunk()
    {
            chunkCount++;

            if (previousWasLongGap)
            {
                previousWasLongGap = false;
                SpawnNormalChunk();
                lastWasSpecial = false;
                return;
            }

            bool spawnSpecial = false;

            if (!lastWasSpecial && chunkCount > 3 && difficulty.ShouldSpawnSpecial())
            {
                spawnSpecial = true;
                Debug.Log("Spawning Special Chunk");
            }
                

            SpecialType? special = null;

            if (spawnSpecial)
                special = RollSpecialTypeBalanced();

            // ถ้า roll ได้ LongGap
            if (special == SpecialType.LongGap && !previousWasLongGap)
            {
                previousWasLongGap = true;
                lastWasSpecial = true; 
                SpawnLongGapChunk();
                
                return;
            }

            // ถ้าเป็น special อื่น
            previousWasLongGap = false;
            if (special != null)
            {
                SpawnSpecialChunk(special.Value);
                lastWasSpecial = true;
            }
            else
            {
                SpawnNormalChunk();
                lastWasSpecial = false;
            }
                
            
        }
    void SpawnLongGapChunk()
    {
        var obj = Instantiate(longGapPrefab, lastExit.position, Quaternion.identity);

        // บอก CharacterManager ว่ามี long gap spawn แล้ว
        CharacterManager.Instance.SpawnedLongGap();

        active.Enqueue(obj);
        lastExit = obj.transform.Find("ExitPoint");
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
                CharacterManager.Instance.SpawnedSlideObstacle();
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
        obj.transform.position = new Vector3(obj.transform.position.x, -4.75f, obj.transform.position.z);

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
            chunk.transform.position += Vector3.back * DistanceManager.Instance.speed * Time.deltaTime;
        }
    }

    void RecycleChunks()
    {
        var first = active.Peek();

        // เช็คว่า chunk หลุดกล้องหรือยัง
        if (first.transform.position.z < -25f)
        {
            active.Dequeue();
            Destroy(first);
            SpawnNextChunk();
        }
    }
    void AdvanceExit()
    {
        float chunkLength = 25f; // ตาม prefab
        lastExit.position += Vector3.forward * chunkLength;
    }



}
