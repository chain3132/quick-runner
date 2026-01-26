using System;
using UnityEngine;

public class LongGapTrigger : MonoBehaviour
{
    public static LongGapTrigger Current;
    
    private void Awake()
    {
        Current = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterManager.Instance.OnEnterLongGapZone();
    }
    private void OnTriggerExit(Collider other)
    {
        
        var playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
        var player = other.gameObject.GetComponent<PlayerController>();
        playerRigidbody.useGravity = true;
        playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        //CharacterManager.Instance.OnExitLongGapZone();
        if (CharacterManager.Instance.state != SyncJumpState.ExecutingLongJump)
        {
            player.failLongJump = true;
        }
    }
    
    
}
