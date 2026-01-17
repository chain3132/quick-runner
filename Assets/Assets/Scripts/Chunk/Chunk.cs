    using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Transform[] lanePoints; // lane0, lane1, lane2
    public ObstacleFactory factory;
    public DifficultyController difficulty;

    void Start()
    {
        GenerateObstaclePattern();
    }

    void GenerateObstaclePattern()
    {
        int pattern = difficulty.GetPattern();   // ตัดสินใจ pattern จาก difficulty

        switch(pattern)
        {
            case 0: 
                SpawnOneLane();
                break;
            case 1:
                SpawnTwoLanes();
                break;
            case 2:
                // ไม่มี obstacle ใน chunk นี้
                break;
        }
    }

    void SpawnOneLane()
    {
        int lane = Random.Range(0,3);
        factory.SpawnBlock(lanePoints[lane]);    }

    void SpawnTwoLanes()
    {
        int a = Random.Range(0,3);
        int b;
        do { b = Random.Range(0,3); } while (b == a);

        factory.SpawnBlock(lanePoints[a]);
        factory.SpawnBlock(lanePoints[b]);
    }
}
