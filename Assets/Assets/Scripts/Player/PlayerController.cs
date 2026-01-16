using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalSpeed = 5f;
    private InputHandler input;
    private bool isPlayer1;

    public void Initialize(InputHandler inputHandler, bool forPlayer1)
    {
        input = inputHandler;
        isPlayer1 = forPlayer1;
    }
    

    private void Update()
    {
        Vector2 move = isPlayer1 ? input.P1Move : input.P2Move;

        float dx = move.x * horizontalSpeed * Time.deltaTime;

        transform.position += new Vector3(dx, 0f, 0f);
    }
}
