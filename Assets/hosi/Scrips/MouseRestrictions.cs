using UnityEngine;

public class MouseRestrictions : MonoBehaviour
{
    public Camera mainCamera;
    public Transform cameraCenter;


    void Update()
    {
        Vector3 centerScreenPos = mainCamera.WorldToViewportPoint(cameraCenter.position);

        Vector3 newPosition = transform.position;

        // Clampèàóù
        if (centerScreenPos.x < 0f)
            newPosition.x += mainCamera.ViewportToWorldPoint(Vector3.right * 0).x - cameraCenter.position.x;
        if (centerScreenPos.x > 1f)
            newPosition.x += mainCamera.ViewportToWorldPoint(Vector3.right * 1).x - cameraCenter.position.x;
        if (centerScreenPos.y < 0f)
            newPosition.y += mainCamera.ViewportToWorldPoint(Vector3.up * 0).y - cameraCenter.position.y;
        if (centerScreenPos.y > 1f)
            newPosition.y += mainCamera.ViewportToWorldPoint(Vector3.up * 1).y - cameraCenter.position.y;

        transform.position = newPosition;
    }
}
