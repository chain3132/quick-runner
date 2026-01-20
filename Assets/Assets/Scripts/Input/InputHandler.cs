using System;
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
        
        
        actions.Player.Confirm_P1.performed += ctx => P1Confirm = true;
        actions.Player.Confirm_P1.canceled += ctx => P1Confirm = false;
        
        
        actions.Player.Confirm_P2.performed += ctx => P2Confirm = true;
        actions.Player.Confirm_P2.canceled += ctx => P2Confirm = false;
        
    }
    void Update()
    {
        Vector2 raw1 = actions.Player.Move_P1.ReadValue<Vector2>();
        P1Move = Vector2.zero;

        if (raw1 != Vector2.zero && actions.Player.Move_P1.WasPressedThisFrame())
            P1Move = raw1.normalized;
        Vector2 raw2 = actions.Player.Move_P2.ReadValue<Vector2>();
        P2Move = Vector2.zero;

        if (raw2 != Vector2.zero && actions.Player.Move_P2.WasPressedThisFrame())
            P2Move = raw2.normalized;
    }
    

    
}
