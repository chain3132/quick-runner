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
        
        var playerController = other.gameObject.GetComponent<PlayerController>();
        var playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
        
        playerController.isPlayerDie = true;
        playerRigidbody.useGravity = true;
        //CharacterManager.Instance.OnExitLongGapZone();
    }
    
    
}
