using System;
using UnityEngine;

public class FallHole : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameManager.Instance.Fail();
    }
}
