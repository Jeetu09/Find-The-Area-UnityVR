using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothTurn : MonoBehaviour
{
    public Transform playerRig;             // XR Rig or Player root
    public float rotationSpeed = 90f;       // degrees per second
    public InputActionProperty rightStick;  // assign "RightHand / Thumbstick" Vector2

    void Update()
    {
        // Read joystick vector2
        Vector2 stickInput = rightStick.action.ReadValue<Vector2>();

        // Only take X axis (left/right)
        float turnInput = stickInput.x;

        if (Mathf.Abs(turnInput) > 0.1f) // add deadzone
        {
            playerRig.Rotate(Vector3.up, turnInput * rotationSpeed * Time.deltaTime);
        }

        // Y axis (stickInput.y) is ignored → so pushing forward/back does nothing
    }
}
