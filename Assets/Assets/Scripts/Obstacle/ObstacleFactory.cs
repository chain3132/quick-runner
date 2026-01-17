using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public GameObject blockPrefab;
    public DifficultyController difficulty;
    public int offsetY = 2;

    public GameObject SpawnBlock(Transform laneParent)
    {
        GameObject obj = Instantiate(blockPrefab, laneParent.position + new Vector3(0,offsetY), Quaternion.identity);
        obj.transform.SetParent(laneParent);   
        return obj;
    }
}
