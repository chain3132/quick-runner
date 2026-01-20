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
    
    
}
