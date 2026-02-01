using UnityEngine;

public class firstarrow : MonoBehaviour
{
    [Header("Arrow Setup")]
    public GameObject arrow1;
    public GameObject player;

    [Header("Animation Settings")]
    public float rotateSpeed = 50f;       // Degrees per second
    public float floatAmplitude = 0.5f;   // Height of float
    public float floatFrequency = 1f;     // Speed of float

    [Header("Proximity Settings")]
    public float hideDistance = 2f;

    private Vector3 startPos;

    void Start()
    {
        if (arrow1 != null)
        {
            arrow1.SetActive(true);
            startPos = arrow1.transform.position;
        }
    }

    void Update()
    {
        if (arrow1 == null || player == null) return;

        // Animate global Y-axis rotation
        arrow1.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        // Animate floating (up and down)
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 floatOffset = new Vector3(0, newY, 0);
        arrow1.transform.position = startPos + floatOffset;

        // Hide arrow if player is close enough
        float distance = Vector3.Distance(player.transform.position, arrow1.transform.position);
        if (distance < hideDistance)
        {
            arrow1.SetActive(false);
        }
    }
}
