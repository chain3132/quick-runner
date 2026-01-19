using System;
using UnityEngine;

public class EnterTunnelTrigger : MonoBehaviour
{
    public static EnterTunnelTrigger Current;

    
    private void Awake()
    {
        Current = this;
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager.Instance.OnEnterSlideZone();
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.gameObject.CompareTag("P1") || other.gameObject.gameObject.CompareTag("P2"))
        {
            CharacterManager.Instance.OnFailedSlide();
        }
        
    }
}
