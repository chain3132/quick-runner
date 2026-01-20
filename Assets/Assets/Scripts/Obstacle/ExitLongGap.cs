using UnityEngine;

public class ExitLongGap : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        CharacterManager.Instance.OnExitLongGapZone();
    }
}
