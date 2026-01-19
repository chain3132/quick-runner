using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public float difficulty = 1f;
    
    void Update()
    {
        difficulty += Time.deltaTime * 0.1f;
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
    public bool ShouldSpawnSpecial()
    {
        if (difficulty < 2f) return false;

        float p = Mathf.Clamp01((difficulty - 2f) * 0.1f); 
        // ถ้า diff=2 => 0% / diff=4 => 20% / diff=6 => 40% 
        //Debug.Log("Special Spawn Probability: " + p);
        return Random.value < p;
    }
}
