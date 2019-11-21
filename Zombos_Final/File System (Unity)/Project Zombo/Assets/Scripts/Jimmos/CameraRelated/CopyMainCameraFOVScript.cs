using UnityEngine;

public class CopyMainCameraFOVScript : MonoBehaviour
{
    // The only function of this script is to sync the fov of the player and gun camera
    private Camera thisCamera;
    private Camera playerCamera;

    void Awake()
    {
        thisCamera = GetComponent<Camera>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        thisCamera.fieldOfView = playerCamera.fieldOfView;
    }
}