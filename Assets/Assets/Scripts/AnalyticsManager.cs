using System.Collections;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager instance;
    float sessionStartTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    float GetPlayTime()
    {
        int playTime = Mathf.RoundToInt(Time.realtimeSinceStartup - sessionStartTime);
        return playTime;
    }
    private async void Initialize() 
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            Debug.Log("Unity Services initialized successfully.");
        }
        else
        {
            Debug.LogError("Failed to initialize Unity Services.");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
        sessionStartTime = Time.realtimeSinceStartup;

    }
    public void HitObstacle(HitType hitType,int distance ,float time)
    {
        string hitTypeString = CheckPlayerDied(hitType);
        CustomEvent hitObstacleEvent = new CustomEvent("RunDistance"){{"causeOfDeath", hitTypeString},{"distance", distance},{"survivalTime", time}};
        AnalyticsService.Instance.RecordEvent(hitObstacleEvent);
    }
    private string CheckPlayerDied(HitType hitType)
    {
        switch (hitType)
        {
            case HitType.Barrier:
                    return "Barrier";
            case HitType.Car:
                return "Car";
            case HitType.Sign:
                return "Sign";
            case HitType.Slide:
                return "Slide";
            case HitType.Wall:
                return "Wall";
            case HitType.LongGap:
                return "LongGap";
            default:
                return "Unknown";
        }
    }
    public void SendSessionEnd(float playTime, bool quitEarly)
    {
        CustomEvent sessionEvent = new CustomEvent("SessionLength")
        {
            { "timeInGame", playTime },
            { "quit_early", quitEarly }
        };

        AnalyticsService.Instance.RecordEvent(sessionEvent);
    }
    
    
    
    public void OnClickExit()
    {
        StartCoroutine(ExitRoutine());
    }

    IEnumerator ExitRoutine()
    {
        float playTime = GetPlayTime();

        SendSessionEnd(playTime, true);

        // รอให้ Analytics มีเวลาส่ง
        yield return new WaitForSecondsRealtime(0.5f);
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void Revive(bool continuePlaying, bool watchAds)
    {
        CustomEvent reviveEvent = new CustomEvent("ReviveRATE")
        {
            { "continuePlaying",  continuePlaying},
            { "watchAds", watchAds }
        };
        AnalyticsService.Instance.RecordEvent(reviveEvent);
    }
}
