using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    [SerializeField] private float failDelay = 1f;
    private MMF_Player _hitFeedBack;
    public void Initialize(MMF_Player hitFeedback)
    {
        _hitFeedBack = hitFeedback;
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("P1" ) || other.collider.CompareTag("P2"))
        {
            GameManager.Instance.isGameOver = true;
            var player = other.collider.GetComponent<PlayerController>();
            player.Dead();
            StartCoroutine(HitSequence(player));
        }
    }
    
    private IEnumerator HitSequence(PlayerController player)
    {
        if (GameManager.Instance.isGameOver)
        {
            yield return null;
        }
        _hitFeedBack.PlayFeedbacks();
        GameManager.Instance.SpawnDrone(player.transform);
        yield return new WaitForSeconds(failDelay);
        GameManager.Instance.Fail();
    }
    
}
