using System;
using UnityEngine;

public class HitObstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("HitObstacle Collided with " + other.collider.name);
        if (other.collider.CompareTag("P1" ) || other.collider.CompareTag("P2"))
        {
            GameManager.Instance.Fail();
        }
    }
}
