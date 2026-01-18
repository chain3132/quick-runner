using UnityEngine;

public class ChunkMover : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}
