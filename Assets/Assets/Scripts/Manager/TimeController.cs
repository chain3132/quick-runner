using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    private Coroutine slowRoutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySlow(float scale, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowRoutine(scale, duration));
    }

    private IEnumerator SlowRoutine(float scale, float duration)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * scale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        slowRoutine = null;
    }
}
