using System;
using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private float failDelay = 0.5f;
    
    private void Start()
    {
        if (hitFeedback == null )
        {
            hitFeedback = FindAnyObjectByType<MMF_Player>();
            Debug.Log(hitFeedback);
        }
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
        
        hitFeedback.PlayFeedbacks();

        yield return new WaitForSeconds(failDelay);

        GameManager.Instance.Fail();
    }
}
