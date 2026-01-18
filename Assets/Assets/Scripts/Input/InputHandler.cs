using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 P1Move { get; private set; }
    public bool P1Confirm { get; private set; }

    public Vector2 P2Move { get; private set; }
    public bool P2Confirm { get; private set; }

    private PlayerInput  actions;

    private void Awake()
    {
        actions = new PlayerInput();
        actions.Player.Enable();
        
        actions.Player.Move_P1.performed += ctx => P1Move = ctx.ReadValue<Vector2>();
        actions.Player.Move_P1.canceled += ctx => P1Move = Vector2.zero;
        actions.Player.Confirm_P1.performed += ctx => P1Confirm = true;
        actions.Player.Confirm_P1.canceled += ctx => P1Confirm = false;
        
        actions.Player.Move_P2.performed += ctx => P2Move = ctx.ReadValue<Vector2>();
        actions.Player.Move_P2.canceled += ctx => P2Move = Vector2.zero;
        actions.Player.Confirm_P2.performed += ctx => P2Confirm = true;
        actions.Player.Confirm_P2.canceled += ctx => P2Confirm = false;
        
    }
}
