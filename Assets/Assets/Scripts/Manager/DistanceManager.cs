using TMPro;
using UnityEngine;

public class DistanceManager : MonoBehaviour
{
    public static DistanceManager Instance;
    
    public float distance;
    public float speed = 8f;
    public TextMeshProUGUI distanceText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        distance += speed * Time.deltaTime;
        distanceText.text = Mathf.FloorToInt(distance).ToString() + " m";
    }
}
