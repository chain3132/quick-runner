using System;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("P1" ) || other.collider.CompareTag("P2"))
        {
            GameManager.Instance.Fail();
        }
    }
}
