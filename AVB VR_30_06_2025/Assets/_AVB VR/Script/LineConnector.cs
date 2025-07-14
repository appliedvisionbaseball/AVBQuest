using UnityEngine;

public class LineConnector : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform objectToFollow;  // 3D object
    public RectTransform uiPopup;     // UI popup in Canvas
    public Camera mainCamera;         // Assign main camera

    void Update()
    {
        if (objectToFollow == null || uiPopup == null || lineRenderer == null) return;

        // Get object world position
        Vector3 worldPosition = objectToFollow.position;

        // Convert world position to screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Convert screen position to UI canvas position
        Vector3 uiPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiPopup.parent as RectTransform, screenPosition, mainCamera, out uiPosition);

        // Update LineRenderer positions
        lineRenderer.SetPosition(0, worldPosition);  // Start at 3D object
        lineRenderer.SetPosition(1, uiPosition);     // End at UI Popup
    }
}
