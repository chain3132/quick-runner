using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnterWallObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnterWallObstacle Triggered");
        Debug.Log(other.GetComponent<Rigidbody>() == null);
        if (other.GetComponent<Rigidbody>() == null)
        {
            var otherRigidbody = other.AddComponent<Rigidbody>();
            otherRigidbody.mass = 100f;
            otherRigidbody.isKinematic = true;
            Debug.Log("Add Rigidbody to Player");
        }
    }
}
