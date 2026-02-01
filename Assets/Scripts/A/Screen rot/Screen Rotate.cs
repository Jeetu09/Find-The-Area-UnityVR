using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRotate : MonoBehaviour
{
    public GameObject Player;
    public GameObject DroneScreen;

    void Update()
    {
        // Get direction from DroneScreen to Player (ignoring y axis)
        Vector3 direction = Player.transform.position - DroneScreen.transform.position;
        direction.y = 0;  // Keep rotation only on XZ plane

        if (direction.sqrMagnitude > 0.001f)
        {
            // Calculate target rotation
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Add 90-degree offset around Y-axis
            Quaternion offsetRotation = Quaternion.Euler(0, 90, 0);

            // Apply combined rotation
            DroneScreen.transform.rotation = lookRotation * offsetRotation;
        }
    }
}
