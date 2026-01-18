using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject lowBlockPrefab;
    public GameObject breakWallPrefab;
    public GameObject longGapPrefab;
    public GameObject tunnelPrefab;
    public DifficultyController difficulty;
    public int offsetY = 2;
    public int offsetYBreakWall = 2;
    public float chunkLength = 25f;

    public GameObject SpawnBlock(Transform laneParent)
    {
        float zOffset = Random.Range(2f, chunkLength - 2f); // กันไม่ให้ชิดหัว-ท้ายเกินไป
        Vector3 pos = laneParent.position + new Vector3(0, offsetY, zOffset);

        GameObject obj = Instantiate(blockPrefab, pos, Quaternion.identity);
        obj.transform.SetParent(laneParent);
        return obj;
    }

    public GameObject SpawnLowBlock(Transform laneParent)
    {
        float zOffset = Random.Range(2f, chunkLength - 2f);
        Vector3 pos = laneParent.position + new Vector3(0, 0, zOffset);

        GameObject obj = Instantiate(lowBlockPrefab, pos, Quaternion.identity);
        obj.transform.SetParent(laneParent);
        return obj;
    }
    
    public GameObject SpawnLongGap(Transform laneParent)
    {
        GameObject obj = Instantiate(lowBlockPrefab, laneParent.position, Quaternion.identity);
        obj.transform.SetParent(laneParent);  
        return obj;
    }

    public GameObject SpawnTunnel(Transform laneParent)
    {
        GameObject obj = Instantiate(tunnelPrefab, laneParent.position, Quaternion.identity);
        obj.transform.SetParent(laneParent);  
        return obj;
    }

    public GameObject SpawnBreakWall(Transform laneParent)
    {
        GameObject obj = Instantiate(breakWallPrefab, laneParent.position + new Vector3(0,offsetYBreakWall,chunkLength / 2), Quaternion.identity);
        obj.transform.SetParent(laneParent);  
        return obj;
    }
}
