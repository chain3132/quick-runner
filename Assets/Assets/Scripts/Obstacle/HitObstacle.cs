using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    [SerializeField] private float failDelay = 0.5f;
    private MMF_Player _hitFeedBack;
    public void Initialize(MMF_Player hitFeedback)
    {
        _hitFeedBack = hitFeedback;
    }
    

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("P1" ) || other.collider.CompareTag("P2"))
        {
            StartCoroutine(HitSequence());
        }
    }
    
    private IEnumerator HitSequence()
    {
        
        _hitFeedBack.PlayFeedbacks();

        yield return new WaitForSeconds(failDelay);

        GameManager.Instance.Fail();
    }
}
