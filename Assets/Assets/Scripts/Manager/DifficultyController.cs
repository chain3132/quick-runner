using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DifficultyController : MonoBehaviour
{
    public int difficulty = 1;
    
    
    public int GetPattern()
    {
        if (difficulty < 2f) return Random.Range(0,2); // 0 or 1
        if (difficulty < 4f) return Random.Range(0,3); // 0,1,2
        return Random.Range(0,2); // ลด pattern empty เมื่อยากขึ้น
    }

    private void Update()
    {
        if (DistanceManager.Instance.distance <= 500f)
        {
            difficulty = 1;
            
        }
        else if (DistanceManager.Instance.distance <= 1500f)
        {
            difficulty = 3;
        }
        else
        {
            difficulty = 5;    
        }
        DistanceManager.Instance.CheckDifficulty(difficulty);
        
    }
    



    public float GetBlockScale()
    {
        return Mathf.Clamp(1f + difficulty * 0.25f, 1f, 3f);
    }
    public bool ShouldSpawnSpecial()
    {
        float p = 0f;

        switch (difficulty)
        {
            case 1:
                p = 0f;      // 0%
                break;

            case 3:
                p = 0.45f;   // 45%
                break;

            case 5:
                p = 0.75f;   // 75%
                break;
        }

        return Random.value < p;
    }
}
