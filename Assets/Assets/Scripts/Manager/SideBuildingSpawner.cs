using System.Collections.Generic;
using UnityEngine;

public class SideBuildingSpawner : MonoBehaviour
{
    [Header("Building Prefabs")]
    public GameObject[] buildingPrefabs;

      [Header("Roots")]
    public Transform leftRoot;
    public Transform rightRoot;

    [Header("Z Range")]
    public float startZ = 0f;
    public float endZ = 120f;
    public float spacing = 10f;

    [Header("X Position")]
    public float leftX = -12f;
    public float rightX = 12f;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float recycleZ = -2f;

    List<GameObject> leftPool = new List<GameObject>();
    List<GameObject> rightPool = new List<GameObject>();

    void Start()
    {
        InitSide(true);
        InitSide(false);
    }

    void Update()
    {
        UpdateSide(leftPool, true);
        UpdateSide(rightPool, false);
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

        ActivateRandomVisual(holder);

        var mover = holder.AddComponent<MovingBuilding>();
        mover.speed = moveSpeed;

        return holder;
    }

    void ActivateRandomVisual(GameObject holder)
    {
        int index = Random.Range(0, holder.transform.childCount);

        for (int i = 0; i < holder.transform.childCount; i++)
        {
            GameObject v = holder.transform.GetChild(i).gameObject;
            v.SetActive(i == index);

            if (i == index)
            {
                AlignToGround(v);
            }
        }
    }
    void AlignToGround(GameObject visual)
    {
        Renderer r = visual.GetComponentInChildren<MeshRenderer>();
        if (r == null) return;

        float bottomY = r.bounds.min.y;
        float offset = visual.transform.position.y - bottomY;

        visual.transform.position += Vector3.up * offset;
    }
}
