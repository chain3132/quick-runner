using UnityEngine;

public class AfterImageSpawner : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public Material afterImageMaterial;
    public float spawnInterval = 0.05f;
    public float lifetime = 0.4f;
    
    float timer;
    public bool active;

    void Update()
    {
        if (!this.active) return;
        timer += Time.deltaTime;
        
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnAfterImage();
        }
        
    }
    public void StartAfterImage()
    {
        this.active = true;
    }

    public void StopAfterImage()
    {
        this.active = false;
    }
    void SpawnAfterImage()
    {
        Mesh bakedMesh = new Mesh();
        skinnedMesh.BakeMesh(bakedMesh);

        GameObject ghost = new GameObject("AfterImage");

        // 1. parent เพื่อ copy transform ให้ตรง
        ghost.transform.SetParent(skinnedMesh.transform.parent, false);

        ghost.transform.localPosition = skinnedMesh.transform.localPosition;
        ghost.transform.localRotation = skinnedMesh.transform.localRotation;
        ghost.transform.localScale    = skinnedMesh.transform.localScale;

        // 2. detach ออกทันที -> กลายเป็น world object
        ghost.transform.SetParent(null, true);
        

        MeshFilter mf = ghost.AddComponent<MeshFilter>();
        mf.sharedMesh = bakedMesh;

        MeshRenderer mr = ghost.AddComponent<MeshRenderer>();

        int count = skinnedMesh.sharedMaterials.Length;
        Material[] mats = new Material[count];
        for (int i = 0; i < count; i++)
            mats[i] = afterImageMaterial; 
        mr.materials = mats;

        AfterImageFade fade = ghost.AddComponent<AfterImageFade>();
        fade.lifetime = lifetime;
    }
}
