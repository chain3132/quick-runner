using UnityEngine;

public class AfterImageFade : MonoBehaviour
{
    public float lifetime = 0.4f;

    float timer;
    MeshRenderer mr;
    MaterialPropertyBlock block;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        block = new MaterialPropertyBlock();
    }

    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Clamp01(1f - (timer / lifetime));

        mr.GetPropertyBlock(block);
        block.SetFloat("_Alpha", alpha);
        mr.SetPropertyBlock(block);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
