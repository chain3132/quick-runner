using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject failUI;
    public GameObject drone;
    public Transform parent;
    public bool isGameOver = false;
    private void Awake()
    {
        Instance = this;
        
    }
    public void Fail()
    {
        Time.timeScale = 0f;
        failUI.SetActive(true);
    }
    public void Restart()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }
    public void SpawnDrone(Transform target)
    {
        Instantiate(drone,parent);
        parent.localPosition = new Vector3(target.position.x, 0, target.position.z);
    }
    
}
