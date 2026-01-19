using UnityEngine;
public enum SyncJumpState
{
    None,
    WaitingZoneTrigger,
    WaitingBothPlayers,
    WaitingJumpInput,
    ExecutingLongJump
}
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public InputHandler input;
    public PlayerController p1;
    public PlayerController p2;
    public int LaneP1 => laneP1;
    public int LaneP2 => laneP2;
    public float[] laneX = new float[] { -2f, 0f, 2f };
    
    private int laneP1 = 1;
    private int laneP2 = 1;

    private bool laneInputLockP1;
    private bool laneInputLockP2;
    SyncJumpState state = SyncJumpState.None;

    
    private bool p1Inside;
    private bool p2Inside;
    private bool p1Jumped;
    private bool p2Jumped;
    float jumpWindow = 1.2f; 
    float jumpTimer;

    
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
    }
    
    void HandleVerticalInput(PlayerController p, float y)
    {
        if (y > 0.5f)
            p.Jump();
        if (y < -0.5f)
            p.Slide();
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
            GameManager.Instance.Fail();
    }
    bool IsPlayerInside(PlayerController p)
    {
        return Vector3.Distance(p.transform.position, LongGapTrigger.Current.transform.position) < 5f;
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
                        GameManager.Instance.Fail();

                        return;
                    }
                    state = SyncJumpState.WaitingJumpInput;
                }
                break;

            case SyncJumpState.WaitingJumpInput:
                jumpTimer -= Time.deltaTime;

                if (p1Jumped && p2Jumped)
                {
                    state = SyncJumpState.ExecutingLongJump;
                    ExecuteLongJump();
                    break;
                }

                if (jumpTimer <= 0f)
                {
                    GameManager.Instance.Fail();
                }
                break;

            case SyncJumpState.ExecutingLongJump:
                // air lock, no lane switching
                break;
        }
    }

    void ExecuteLongJump()
    {
        p1.ExecuteLongJump();
        p2.ExecuteLongJump();
    }

    void ResetFlags()
    {
        p1Inside = p2Inside = false;
        p1Jumped = p2Jumped = false;
    }
    public void OnLongJumpLanded()
    {
        state = SyncJumpState.None;
    }
}
