using UnityEngine;

public class ExitLongGap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        var player = other.gameObject.GetComponent<PlayerController>();
        var playerRigidbody = other.gameObject.GetComponent<Rigidbody>();
        player.EndLongJump();
        player.failLongJump = false;
        player.ResetAfterLongJump();
        CharacterManager.Instance.OnLongJumpLanded();
        

        playerRigidbody.useGravity = false;

    }
}
