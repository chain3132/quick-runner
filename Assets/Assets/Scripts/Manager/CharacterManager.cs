using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public InputHandler input;
    public PlayerController p1;
    public PlayerController p2;

    private void Start()
    {
        p1.Initialize(input, true);
        p2.Initialize(input, false);
    }
}
