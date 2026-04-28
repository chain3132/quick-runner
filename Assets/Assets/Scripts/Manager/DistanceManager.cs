using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    public static DistanceManager Instance;
    
    public float distance;
    public static float reviveDistance = 0f;    
    public float speed = 13f;
    public float time = 0;
    public TextMeshProUGUI distanceText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        distance = reviveDistance;
        reviveDistance = 0f;
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        distance += speed * Time.deltaTime;
        time += Time.deltaTime;
        distanceText.text = Mathf.FloorToInt(distance).ToString() + " m";
    }
    public void CheckDifficulty(int d)
    {
        switch (d)
        {
            case 1:
                speed = 13f;
                break;
            case 3:
                speed = 20f;
                break;
            case 5:
                speed = 30f;
                break;
        }

        
    }
    
}
