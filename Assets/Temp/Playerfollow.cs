using UnityEngine;

public class Playerfollow : MonoBehaviour
{
    [Header("Focus Control")]
    public Transform HelpingRoboRectObj; // Robot object
    public Transform targetObject;       // Player or target

    private void Update()
    {
        if (HelpingRoboRectObj != null && targetObject != null)
        {
            // Get direction on the XZ plane only (ignore vertical difference)
            Vector3 direction = targetObject.position - HelpingRoboRectObj.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f) // Prevent zero direction error
            {
                // Calculate target rotation around global Y-axis
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

                // Apply only Y rotation globally
                HelpingRoboRectObj.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            }
        }
    }
}

