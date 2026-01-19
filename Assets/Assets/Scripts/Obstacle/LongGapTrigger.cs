using UnityEngine;

public class LongGapTrigger : MonoBehaviour
{
    public static LongGapTrigger Current;

    
    private void Awake()
    {
        Current = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Long Gap Zone");
        CharacterManager.Instance.OnEnterLongGapZone();
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterManager.Instance.OnExitLongGapZone();
    }
}
