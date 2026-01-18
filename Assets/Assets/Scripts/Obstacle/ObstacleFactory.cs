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
    

    public GameObject SpawnBlock(Transform laneParent)
    {
        GameObject obj = Instantiate(blockPrefab, laneParent.position + new Vector3(0,offsetY), Quaternion.identity);
        obj.transform.SetParent(laneParent);   
        return obj;
    }
    public GameObject SpawnLowBlock(Transform laneParent)
    {
        GameObject obj = Instantiate(lowBlockPrefab, laneParent.position, Quaternion.identity);
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
        GameObject obj = Instantiate(breakWallPrefab, laneParent.position, Quaternion.identity);
        obj.transform.SetParent(laneParent);  
        return obj;
    }
}
