using UnityEngine;

public class CheckEnable : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("----> On enable");
    }

    private void OnDisable()
    {
        Debug.Log("----> On Disable");
    }
}
