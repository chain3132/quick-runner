using UnityEngine;
using UnityEngine.EventSystems;

public enum MobileButtonPlayer { P1, P2 }
public enum MobileButtonAction { Left, Right, Up, Down, Confirm }

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private MobileButtonPlayer player;
    [SerializeField] private MobileButtonAction action;

    private InputHandler input;

    private void Start()
    {
        input = FindFirstObjectByType<InputHandler>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (action == MobileButtonAction.Confirm)
        {
            if (player == MobileButtonPlayer.P1) input.OnP1ConfirmDown();
            else                                 input.OnP2ConfirmDown();
            return;
        }

        Vector2 dir = action switch
        {
            MobileButtonAction.Left  => Vector2.left,
            MobileButtonAction.Right => Vector2.right,
            MobileButtonAction.Up    => Vector2.up,
            MobileButtonAction.Down  => Vector2.down,
            _                        => Vector2.zero
        };

        if (player == MobileButtonPlayer.P1) input.OnP1Move(dir);
        else                                 input.OnP2Move(dir);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (action != MobileButtonAction.Confirm) return;

        if (player == MobileButtonPlayer.P1) input.OnP1ConfirmUp();
        else                                 input.OnP2ConfirmUp();
    }
}
