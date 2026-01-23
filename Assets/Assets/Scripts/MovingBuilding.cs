using UnityEngine;

public class MovingBuilding : MonoBehaviour
{
    public float speed = 10f;
    public float destroyZ = -20f;

    void Update()
    {
        speed = DistanceManager.Instance.speed;
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
