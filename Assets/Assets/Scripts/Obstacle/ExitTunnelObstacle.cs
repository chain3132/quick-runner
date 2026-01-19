using System;
using UnityEngine;

public class ExitTunnelObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager.Instance.OnSlideExecuted();
    }
}
