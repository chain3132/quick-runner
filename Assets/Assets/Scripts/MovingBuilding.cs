using System;
using UnityEngine;

public class MovingBuilding : MonoBehaviour
{
    public float speed ;
    public float destroyZ = -20f;

    

    void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
