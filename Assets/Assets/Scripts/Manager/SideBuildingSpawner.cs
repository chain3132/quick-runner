using System.Collections.Generic;
using UnityEngine;

public class SideBuildingSpawner : MonoBehaviour
{
    [Header("Building Prefabs")]
    public GameObject[] buildingPrefabs;
    public GameObject[] treesPrefabs;

      [Header("Roots")]
    public Transform leftRoot;
    public Transform rightRoot;
    
    public Transform treesLeftRoot;
    public Transform treesRightRoot;

    [Header("Z Range")]
    public float startZ = 0f;
    public float endZ = 120f;
    public float spacing = 10f;
    [Header("Tree Random Spacing")]
    public float treeMinSpacing = 8f;
    public float treeMaxSpacing = 20f;

    [Header("X Position")]
    public float leftX = -12f;
    public float rightX = 12f;
    
    [Header("Tree X Range")]
    public float treesLeftMinX = -20f;
    public float treesLeftMaxX = -10f;

    public float treesRightMinX = 10f;
    public float treesRightMaxX = 20f;
    
    

    [Header("Movement")]
    public float recycleZ = -2f;

    List<GameObject> leftPool = new List<GameObject>();
    List<GameObject> rightPool = new List<GameObject>();

    List<GameObject> treesLeftPool = new List<GameObject>();
    List<GameObject> treesRightPool = new List<GameObject>();

    void Start()
    {
        InitSide(true);
        InitSide(false);
        
        InitTrees(true);
        InitTrees(false);
    }

    void Update()
    {
        UpdateSide(leftPool, true);
        UpdateSide(rightPool, false);
        
        
        UpdateTrees(treesLeftPool, true);
        UpdateTrees(treesRightPool, false);
    }
    GameObject CreateTreeHolder(float x, float z, Transform parent)
    {
        GameObject holder = new GameObject("TreeHolder");
        holder.transform.SetParent(parent);
        holder.transform.position = new Vector3(x, 1, z);

        GameObject tree = Instantiate(
            treesPrefabs[Random.Range(0, treesPrefabs.Length)],
            holder.transform
        );

        AlignToGround(tree);

        var mover = holder.AddComponent<MovingBuilding>();
        mover.speed = 13f;

        return holder;
    }
    void UpdateTrees(List<GameObject> pool, bool isLeft)
    {
        GameObject first = pool[0];

        if (first.transform.position.z < recycleZ)
        {
            GameObject last = pool[pool.Count - 1];

            float minX = isLeft ? treesLeftMinX : treesRightMinX;
            float maxX = isLeft ? treesLeftMaxX : treesRightMaxX;

            float newX = Random.Range(minX, maxX);
            float randomSpacing = Random.Range(treeMinSpacing, treeMaxSpacing);
            float newZ = last.transform.position.z + randomSpacing;

            first.transform.position = new Vector3(newX, 0f, newZ);

            pool.RemoveAt(0);
            pool.Add(first);
        }
    }
    void InitTrees(bool isLeft)
    {
        Transform root = isLeft ? treesLeftRoot : treesRightRoot;
        List<GameObject> pool = isLeft ? treesLeftPool : treesRightPool;

        float minX = isLeft ? treesLeftMinX : treesRightMinX;
        float maxX = isLeft ? treesLeftMaxX : treesRightMaxX;

        float z = startZ;

        while (z <= endZ)
        {
            float x = Random.Range(minX, maxX);

            GameObject holder = CreateTreeHolder(x, z, root);
            pool.Add(holder);

            // 🔀 random spacing
            z += Random.Range(treeMinSpacing, treeMaxSpacing);
        }
    }

    void InitSide(bool isLeft)
    {
        float x = isLeft ? leftX : rightX;
        Transform root = isLeft ? leftRoot : rightRoot;
        List<GameObject> pool = isLeft ? leftPool : rightPool;

        for (float z = startZ; z <= endZ; z += spacing)
        {
            GameObject holder = CreateHolder(x, z, root);
            pool.Add(holder);
        }
    }

    void UpdateSide(List<GameObject> pool, bool isLeft)
    {
        GameObject first = pool[0];

        if (first.transform.position.z < recycleZ)
        {
            GameObject last = pool[pool.Count - 1];
            float newZ = last.transform.position.z + spacing;

            float x = isLeft ? leftX : rightX;

            first.transform.position = new Vector3(x, 0f, newZ);
            ActivateRandomVisual(first);

            pool.RemoveAt(0);
            pool.Add(first);
        }
    }

    GameObject CreateHolder(float x, float z, Transform parent)
    {
        GameObject holder = new GameObject("BuildingHolder");
        holder.transform.SetParent(parent);
        holder.transform.position = new Vector3(x, 0, z);

        foreach (GameObject prefab in buildingPrefabs)
        {
            GameObject visual = Instantiate(prefab, holder.transform);
            visual.SetActive(false);
        }

        holder.AddComponent<BuildingHolderData>();

        ActivateRandomVisual(holder);

        var mover = holder.AddComponent<MovingBuilding>();
        mover.speed = 13f;

        return holder;
    }

    void ActivateRandomVisual(GameObject holder)
    {
        var data = holder.GetComponent<BuildingHolderData>();
        int count = holder.transform.childCount;

        if (count <= 1)
        {
            holder.transform.GetChild(0).gameObject.SetActive(true);
            return;
        }

        int index;
        do
        {
            index = Random.Range(0, count);
        }
        while (index == data.lastIndex);

        data.lastIndex = index;

        for (int i = 0; i < count; i++)
        {
            GameObject v = holder.transform.GetChild(i).gameObject;
            bool active = (i == index);
            v.SetActive(active);

            if (active)
                AlignToGround(v);
        }
    }
    void AlignToGround(GameObject visual)
    {
        var offsetData = visual.GetComponent<BuildingYOffset>();

        float targetY = 0f;
        if (offsetData != null)
        {
            targetY = offsetData.yOffset;
        }

        visual.transform.localPosition = new Vector3(
            0f,
            targetY,
            0f
        );
    }
}
