using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public InputHandler input;
    public PlayerController p1;
    public PlayerController p2;

    public float[] laneX = new float[] { -2f, 0f, 2f };

    private int laneP1 = 1;
    private int laneP2 = 1;

    private bool laneInputLockP1;
    private bool laneInputLockP2;


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
    }
    void HandleVerticalInput(PlayerController p, float y)
    {
        if (y > 0.5f)
        {
            p.Jump();
        }

        if (y < -0.5f)
        {
            p.Slide();
        }
    }
    void HandleLaneInput(ref int lane, ref bool locked, float x)
    {
        if (!locked)
        {
            if (x > 0.5f) { lane = Mathf.Clamp(lane + 1, 0, laneX.Length - 1); locked = true; }
            if (x < -0.5f) { lane = Mathf.Clamp(lane - 1, 0, laneX.Length - 1); locked = true; }
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
}
