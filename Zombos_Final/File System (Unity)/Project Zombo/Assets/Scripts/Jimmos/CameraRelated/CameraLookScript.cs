using UnityEngine;

public class CameraLookScript : MonoBehaviour
{
    [Header("Sensitivity for camera")]
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    [Header("Clamp values for camera")]
    public float minimumX = -60F;
    public float maximumX = 60F;

    public float lookSmoothness = 0.1f;

    public float rotationY { get; private set; }
    public float rotationX { get; private set; }
    private float currentXRotation;
    private float currentYRotation;
    private float xRotationVelocity = 0f;
    private float yRotationVelocity = 0f;

    public float currentAimRatio { private get; set; } // This is public so that GunScript can decrease the sensitivity, thereby giving some weight to the gun.
    private float defaultCameraAngle = 60f; // Hardcoded to 60 if we change FOV !CHANGE ME!
    public float currentTargetCameraAngle { private get; set; } // This is public so that GunScript can udjust the FOV of our camera, thereby zooming in/out.
    private float ratioZoom = 1f;
    private float ratioZoomVelocity;
    [HideInInspector]
    public float zoomLatency = 0.2f; //This is public because I don't want the zooming coefficient to be "on" the camera, but at the actual thing that uses zooming (ex Gunscript)

    private InputManager input;
    private Camera thisCamera;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        thisCamera = gameObject.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked; //Locks cursor in game window 
        Cursor.visible = false;

        //Sets initial rotation to the one we have at inspector
        rotationY = transform.rotation.eulerAngles.y;
        rotationX = transform.rotation.eulerAngles.x;
        currentYRotation = transform.rotation.eulerAngles.y;
        currentXRotation = transform.rotation.eulerAngles.x;
        currentAimRatio = 1f; // If I don't set that to 1 then at beggining of game it starts from 0 which then updates to whatever, which means that our camera "jiggles" which we don't want
    }

    void Update()
    {
        if (currentAimRatio == 1) // If currentAimRatio is anything other than 1 it basicly means that we are aimed in
        {
            ratioZoom = Mathf.SmoothDamp(ratioZoom, 1f, ref ratioZoomVelocity, zoomLatency);
        }
        else
        {
            ratioZoom = Mathf.SmoothDamp(ratioZoom, 0f, ref ratioZoomVelocity, zoomLatency);
        }

        thisCamera.fieldOfView = Mathf.Lerp(currentTargetCameraAngle, defaultCameraAngle, ratioZoom);

        rotationY += input.MouseX * sensitivityY * currentAimRatio;
        rotationX -= input.MouseY * sensitivityX * currentAimRatio;
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX); // Clamps the rotation on the X axis
        currentXRotation = Mathf.SmoothDamp(currentXRotation, rotationX, ref xRotationVelocity, lookSmoothness); // Interpolates 
        currentYRotation = Mathf.SmoothDamp(currentYRotation, rotationY, ref yRotationVelocity, lookSmoothness); //  SAME
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
    }
}