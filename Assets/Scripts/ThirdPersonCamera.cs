using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public Transform pivot;
    public float mouseSensitivity = 3f;
    public float clampAngle = 70f;

    private float rotationX = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation (player + pivot rotate together)
        player.Rotate(Vector3.up, mouseX);

        // Vertical rotation (only pivot rotates)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -clampAngle, clampAngle);
        pivot.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}