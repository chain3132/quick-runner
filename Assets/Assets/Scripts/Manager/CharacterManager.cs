using UnityEngine;
public enum SyncJumpState
{
    None,
    WaitingZoneTrigger,
    WaitingBothPlayers,
    WaitingJumpInput,
    ExecutingLongJump
}
public enum SyncSlideState
{
    None,
    WaitingZoneTrigger,
    WaitingBothPlayers,
    WaitingSlideInput,
    ExecutingSlide
}
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public InputHandler input;
    
    public PlayerController p1;
    public PlayerController p2;
    public GameObject Ball;
    public int LaneP1 => laneP1;
    public int LaneP2 => laneP2;
    public float[] laneX = new float[] { -2f, 0f, 2f };
    
    private int laneP1 = 1;
    private int laneP2 = 1;

    private bool laneInputLockP1;
    private bool laneInputLockP2;
    public SyncJumpState state = SyncJumpState.None;
    public SyncSlideState slideState = SyncSlideState.None;
    public System.Action OnBothPlayersSameLane;
    public System.Action OnPlayersSeparated;
    [Header("Camera Sync Settings")]
    public float sameLaneDurationRequired = 1f;
    public bool cameraTriggered = false;
    private float sameLaneTimer = 0f;
    
    public bool p1Inside;
    public bool p2Inside;
    private bool p1Jumped;
    private bool p2Jumped;
    private bool p1Slided;
    private bool p2Slided;
    float jumpTimer = 2f;
    
    
    
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        p1.targetX = laneX[laneP1];
        p2.targetX = laneX[laneP2];
    }
    
    private void Update()
    {
        Vector2 m1 = input.P1Move;
        Vector2 m2 = input.P2Move;

        HandleLaneInput(ref laneP1, ref laneInputLockP1, m1.x);
        HandleLaneInput(ref laneP2, ref laneInputLockP2, m2.x);
        
        HandleVerticalInput(p1, m1.y);
        HandleVerticalInput(p2, m2.y);
        ApplyOffsetLogic();
        TickSyncJump();
        TickSyncSlide();
        TickSameLaneCameraTrigger();

    }
    
    void HandleVerticalInput(PlayerController p, float y)
    {
        if (y > 0.5f)
        {
            p.Jump();
            if (state == SyncJumpState.WaitingJumpInput)
            {
                if (p == p1) p1Jumped = true;
                if (p == p2) p2Jumped = true;
            }
        }

        if (y < -0.5f)
        {
            p.Slide();
            if (slideState == SyncSlideState.WaitingSlideInput)
            {
                if (p == p1) p1Slided = true;
                if (p == p2) p2Slided = true;
                Debug.Log(p2Slided);
                Debug.Log(p1Slided);
            }
        }
          
    }
    
    void HandleLaneInput(ref int lane, ref bool locked, float x)
    {
        if (!locked)
        {
            if (x > 0.5f)
            {
                lane = Mathf.Clamp(lane + 1, 0, laneX.Length - 1); locked = true;

            }

            if (x < -0.5f)
            {
                lane = Mathf.Clamp(lane - 1, 0, laneX.Length - 1); locked = true;

            }
        }
        else
        {
            if (Mathf.Abs(x) < 0.1f) locked = false;
        }
    }

    void ApplyOffsetLogic()
    {
        if (laneP1 == laneP2)
        {
            float c = laneX[laneP1];
            p1.targetX = c - 0.5f;
            p2.targetX = c + 0.5f;
        }
        else
        {
            p1.targetX = laneX[laneP1];
            p2.targetX = laneX[laneP2];
        }
    }
    public void SpawnedLongGap()
    {
        state = SyncJumpState.WaitingZoneTrigger;
        ResetFlags();
    }
    public void SpawnedSlideObstacle()
    {
        slideState = SyncSlideState.WaitingZoneTrigger;
        ResetFlags();
    }
    public void OnEnterSlideZone()
    {
        if (slideState == SyncSlideState.WaitingZoneTrigger)
            slideState = SyncSlideState.WaitingBothPlayers;

        if (!p1Inside ) p1Inside = true;
        if (!p2Inside ) p2Inside = true;
    }
    public void OnExitTunnelZone()
    {
        if (slideState != SyncSlideState.ExecutingSlide)
        {
            Debug.Log("Exited Slide Zone too early");
            GameManager.Instance.Fail();
        }
            
    }
    public void OnEnterLongGapZone()
    {
        if (state == SyncJumpState.WaitingZoneTrigger)
            state = SyncJumpState.WaitingBothPlayers;

        if (!p1Inside && IsPlayerInside(p1)) p1Inside = true;
        if (!p2Inside && IsPlayerInside(p2)) p2Inside = true;
    }

    public void OnExitLongGapZone()
    {
        if (state != SyncJumpState.ExecutingLongJump)
        {
            Debug.Log("Exited Long Gap Zone too early");
            GameManager.Instance.Fail();
        }
            
    }
    bool IsPlayerInsideSlideZone(PlayerController p)
    {
        Debug.Log(Vector3.Distance(p.transform.position, EnterTunnelTrigger.Current.transform.position) < 5f);
        return Vector3.Distance(p.transform.position, EnterTunnelTrigger.Current.transform.position) < 5f;
    }
    bool IsPlayerInside(PlayerController p)
    {
        return Vector3.Distance(p.GetComponent<Rigidbody>().position, LongGapTrigger.Current.transform.position) < 30f;
    }

    void TickSyncJump()
    {
        switch (state)
        {
            
            case SyncJumpState.WaitingBothPlayers:
                if (p1Inside && p2Inside)
                {
                    if (laneP1 != laneP2)
                    {
                        return;
                    }
                    state = SyncJumpState.WaitingJumpInput;
                }
                break;

            case SyncJumpState.WaitingJumpInput:
                jumpTimer -= Time.deltaTime;
                Debug.Log($"Waiting for both players to jump. Time left: {jumpTimer}");
                if (p1Jumped && p2Jumped)
                {
                    Debug.Log("Both players jumped!");
                    state = SyncJumpState.ExecutingLongJump;
                    ExecuteLongJump();
                    break;
                }

                if (jumpTimer <= 0f)
                {
                    Debug.Log("Failed to jump in time");
                    GameManager.Instance.Fail();
                }
                break;

            case SyncJumpState.ExecutingLongJump:
                // air lock, no lane switching
                break;
        }
    }
    void TickSyncSlide()
    {
        switch (slideState)
        {
            case SyncSlideState.WaitingBothPlayers:
                if (p1Inside && p2Inside)
                {
                    if (laneP1 != laneP2)
                    {
                        return;
                    }
                    slideState = SyncSlideState.WaitingSlideInput;
                }
                break;

            case SyncSlideState.WaitingSlideInput:
                if (p1Slided && p2Slided)
                {
                    Debug.Log("Both players slid!");
                    slideState = SyncSlideState.ExecutingSlide;
                    ExecuteSlide();
                    break;
                }
                break;

            case SyncSlideState.ExecutingSlide:
                // air lock, no lane switching
                break;
        }
    }
    void ExecuteLongJump()
    {
        Debug.Log("Both players jumped the long gap!");
        p1.ExecuteLongJump();
        p2.ExecuteLongJump();
    }
    
    void ExecuteSlide()
    {
        p1.gameObject.SetActive(false);
        p2.gameObject.SetActive(false);
        Ball.SetActive(true);
        
    }
    void ResetFlags()
    {
        p1Inside = p2Inside = false;
        p1Jumped = p2Jumped = false;
        p1Slided = p2Slided = false;
        
        
    }
    public void OnLongJumpLanded()
    {
        state = SyncJumpState.None;
    }
    public void OnSlideExecuted()
    {
        slideState = SyncSlideState.None;
        p1.gameObject.SetActive(true);
        p2.gameObject.SetActive(true);
        Ball.SetActive(false);
    }
    public void OnFailedSlide()
    {
        Debug.Log("Failed to slide in time");
        
        GameManager.Instance.Fail();
    }
    
    void TickSameLaneCameraTrigger()
    {
        if (laneP1 == laneP2)
        {
            sameLaneTimer += Time.deltaTime;

            if (!cameraTriggered && sameLaneTimer >= sameLaneDurationRequired)
            {
                cameraTriggered = true;
                OnBothPlayersSameLane?.Invoke();
            }
        }
        else
        {
            if (cameraTriggered)
            {
                OnPlayersSeparated?.Invoke();
            }

            sameLaneTimer = 0f;
            cameraTriggered = false;
        }
    }
}
