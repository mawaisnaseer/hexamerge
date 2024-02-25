using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaling : MonoBehaviour
{
    public float targetAspectRatio = 16f / 9f; // Choose your target aspect ratio
    public float gameWorldWidth = 10f; // Adjust this based on your game's world dimensions
    public float gameWorldHeight = 5f; // Adjust this based on your game's world dimensions

    void Start()
    {
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;
        float screenAspectRatio = screenWidth / screenHeight;

        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float viewportWidth, viewportHeight;
            if (screenAspectRatio > targetAspectRatio)
            {
                // Screen is wider, adjust height
                viewportHeight = screenHeight;
                viewportWidth = screenHeight * targetAspectRatio;
            }
            else
            {
                // Screen is taller or equal, adjust width
                viewportWidth = screenWidth;
                viewportHeight = screenWidth / targetAspectRatio;
            }

            // Calculate scaling factors
            float scaleX = viewportWidth / gameWorldWidth;
            float scaleY = viewportHeight / gameWorldHeight;

            // Apply scaling factors to the camera
            mainCamera.orthographicSize = gameWorldHeight / 2f;
            mainCamera.transform.position = new Vector3(gameWorldWidth / 2f, gameWorldHeight / 2f, -10f);
        }
    }
}
