using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 P1Move { get; private set; }
    public bool P1Confirm { get; private set; }

    public Vector2 P2Move { get; private set; }
    public bool P2Confirm { get; private set; }

    private PlayerInput actions;

    private bool p1MobileMoveReady;
    private Vector2 p1MobileDir;
    private bool p2MobileMoveReady;
    private Vector2 p2MobileDir;

    private void Awake()
    {
        actions = new PlayerInput();
        actions.Player.Enable();

        actions.Player.Confirm_P1.performed += ctx => P1Confirm = true;
        actions.Player.Confirm_P1.canceled  += ctx => P1Confirm = false;

        actions.Player.Confirm_P2.performed += ctx => P2Confirm = true;
        actions.Player.Confirm_P2.canceled  += ctx => P2Confirm = false;
    }

    void Update()
    {
        // P1 keyboard
        Vector2 raw1 = actions.Player.Move_P1.ReadValue<Vector2>();
        P1Move = Vector2.zero;
        if (raw1 != Vector2.zero && actions.Player.Move_P1.WasPressedThisFrame())
            P1Move = raw1.normalized;

        // P1 on-screen button (consumed once per tap)
        if (p1MobileMoveReady)
        {
            P1Move = p1MobileDir;
            p1MobileMoveReady = false;
        }

        // P2 keyboard
        Vector2 raw2 = actions.Player.Move_P2.ReadValue<Vector2>();
        P2Move = Vector2.zero;
        if (raw2 != Vector2.zero && actions.Player.Move_P2.WasPressedThisFrame())
            P2Move = raw2.normalized;

        // P2 on-screen button (consumed once per tap)
        if (p2MobileMoveReady)
        {
            P2Move = p2MobileDir;
            p2MobileMoveReady = false;
        }
    }

    // --- Called by MobileButton ---
    public void OnP1Move(Vector2 dir) { p1MobileDir = dir; p1MobileMoveReady = true; }
    public void OnP2Move(Vector2 dir) { p2MobileDir = dir; p2MobileMoveReady = true; }

    public void OnP1ConfirmDown() => P1Confirm = true;
    public void OnP1ConfirmUp()   => P1Confirm = false;
    public void OnP2ConfirmDown() => P2Confirm = true;
    public void OnP2ConfirmUp()   => P2Confirm = false;
}
