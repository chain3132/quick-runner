using MoreMountains.Feedbacks;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float forwardDistance = 5f;
    public float moveDuration = 1f;

    private Vector3 basePosition;
    private Vector3 targetForwardPosition;

    private float t;
    private bool movingForward;
    private bool movingBack;
    public MMF_Player cameraShake;
    private Vector3 moveStartPos;
    private Vector3 moveEndPos;
    private bool moving;

    private bool reachedForward;

    void Start()
    {
        // basePosition = transform.position;
        // targetForwardPosition = basePosition + new Vector3(0, 0, forwardDistance);
        //
        // CharacterManager.Instance.OnBothPlayersSameLane += HandleForward;
        // CharacterManager.Instance.OnPlayersSeparated += HandleReturn;
    }

    // void HandleForward()
    // {
    //     if (reachedForward) return;
    //
    //     moving = true;
    //     t = 0f;
    //     moveStartPos = transform.position;
    //     moveEndPos = targetForwardPosition;
    // }

    // void HandleReturn()
    // {
    //     if (reachedForward) return;
    //
    //     moving = true;
    //     t = 0f;
    //     moveStartPos = transform.position;
    //     moveEndPos = basePosition;
    // }

    // void Update()
    // {
    //     if (!moving) return;
    //
    //     t += Time.deltaTime / moveDuration;
    //
    //     transform.position = Vector3.Lerp(moveStartPos, moveEndPos, Mathf.Clamp01(t));
    //
    //     if (t >= 1f)
    //     {
    //         moving = false;
    //
    //         // แพ้เมื่อถึงปลายทาง forward
    //         if (transform.position == targetForwardPosition)
    //         {
    //             reachedForward = true;
    //             GameOver();
    //         }
    //     }
    // }
    //
    // void GameOver()
    // {
    //     GameManager.Instance.Fail();
    // }
    
    public void ShakeCamera()
    {
        cameraShake.PlayFeedbacks();
    }
}
