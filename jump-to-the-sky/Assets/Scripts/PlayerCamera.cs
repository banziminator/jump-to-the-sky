using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform player;
    public float offsetY = 1.0f;

    private void Update()
    {
        if (player != null && mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.WorldToViewportPoint(player.position);

            if (cameraPosition.y < 0.0f || cameraPosition.y > 1.0f)
            {
                SwitchCamera();
            }
        }
    }

    private void SwitchCamera()
    {
        if (mainCamera.enabled)
        {
            mainCamera.enabled = false;
            // Switch to a different camera or perform other actions here
        }
        else
        {
            mainCamera.enabled = true;
            // Switch back to the main camera or perform other actions here
        }
    }
}