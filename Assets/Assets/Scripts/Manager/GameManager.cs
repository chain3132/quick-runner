using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject failUI;
    public GameObject drone;
    public Transform parent;
    public bool isGameOver = false;
    [SerializeField] public PlayerController player1;
    [SerializeField] private PlayerController player2;
    AudioManager audioManager;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        
    }
    public void Fail()
    {
        audioManager.PlaySFX(audioManager.dieSFX);
        Time.timeScale = 0f;
        failUI.SetActive(true);
    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void Revive()
    {
        DistanceManager.reviveDistance = DistanceManager.Instance.distance;
        Restart();
    }
    public void Restart()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        Debug.Log("Restarting Game...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void SpawnDrone(Transform target,bool parented = true)
    {
        if (parented)
        {
            Instantiate(drone,parent);
        }
        else
        {
            Instantiate(drone);
        }
        
        parent.localPosition = new Vector3(target.position.x, 0, target.position.z);
    }

    public void CheckPlayerDied()
    {
        player1.PlayPrayAnimation();
        player2.PlayPrayAnimation();
    }
}
