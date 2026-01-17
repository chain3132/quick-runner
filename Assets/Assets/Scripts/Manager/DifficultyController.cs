using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public float difficulty = 1f;

    void Update()
    {
        difficulty += Time.deltaTime * 0.05f;
    }

    public int GetPattern()
    {
        if (difficulty < 2f) return Random.Range(0,2); // 0 or 1
        if (difficulty < 4f) return Random.Range(0,3); // 0,1,2
        return Random.Range(0,2); // ลด pattern empty เมื่อยากขึ้น
    }

    public float GetBlockScale()
    {
        return Mathf.Clamp(1f + difficulty * 0.25f, 1f, 3f);
    }
}
