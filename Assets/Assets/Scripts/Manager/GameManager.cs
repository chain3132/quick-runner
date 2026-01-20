using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject failUI;
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
}
