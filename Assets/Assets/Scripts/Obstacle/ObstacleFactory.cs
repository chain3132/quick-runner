using MoreMountains.Feedbacks;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject lowBlockPrefab;
    public GameObject breakWallPrefab;
    public GameObject longGapPrefab;
    public GameObject tunnelPrefab;
    public DifficultyController difficulty;
    public float offsetY = 2;
    public int offsetYBreakWall = 2;
    public float chunkLength = 25f;
    public float offsetYLowBlock =2;
    public float offsetYTunnelWall = 0.5f;
    [SerializeField]
    public MMF_Player hitFeedBack; 
    public GameObject SpawnBlockAt(Transform laneParent, float z)
    {
        Vector3 pos = laneParent.position + new Vector3(0, offsetY, z);
        GameObject obj = Instantiate(blockPrefab, pos, blockPrefab.transform.rotation);
        var obstacle = obj.GetComponent<HitObstacle>();

        obstacle.Initialize(hitFeedBack);

        obj.transform.SetParent(laneParent);
        return obj;
    }
    public void SpawnTypeAt(ObstacleType type, Transform laneParent, float z )
    {
        switch (type)
        {
            case ObstacleType.Jump:
                SpawnBlockAt(laneParent, z);
                break;
            case ObstacleType.Slide:
                SpawnLowBlockAt(laneParent, z);
                break;
        }
    }
    public GameObject SpawnLowBlockAt(Transform laneParent, float z)
    {
        Vector3 pos = laneParent.position + new Vector3(0, offsetYLowBlock, z);
        GameObject obj = Instantiate(lowBlockPrefab, pos, lowBlockPrefab.transform.rotation);
        var obstacle = obj.GetComponent<HitObstacle>();
        obstacle.Initialize(hitFeedBack);
        obj.transform.SetParent(laneParent);
        obj.transform.localPosition += new Vector3(-0.1f, 0, 0);
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
        GameObject obj = Instantiate(tunnelPrefab, laneParent.position + new Vector3(0,offsetYTunnelWall,chunkLength / 2), tunnelPrefab.transform.rotation);
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
