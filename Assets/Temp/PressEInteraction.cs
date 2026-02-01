using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // For new Input System

[System.Serializable]
public class PressEElement
{
    public GameObject targetObject;   // Object to check distance
    public GameObject pressEObject;   // Floating 'Press E' indicator
    public GameObject uiPanel;        // UI shown when E is pressed
}

public class PressEInteraction : MonoBehaviour
{
    [Header("Arrow")]
    public GameObject arrow;
    public float arrowDisableDistance = 2f;
    public float arrowFloatAmplitude = 0.2f;
    public float arrowFloatFrequency = 1f;
    public float arrowRotationSpeed = 30f;

    [Header("Player Reference")]
    public GameObject player;

    [Header("Press E Elements")]
    public List<PressEElement> elements = new List<PressEElement>();

    [Header("Settings")]
    public float interactionDistance = 3f;
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    [Header("Door Animations")]
    public Animator leftDoorAnimator;
    public string leftDoorTrigger = "OpenLeft";
    public Animator rightDoorAnimator;
    public string rightDoorTrigger = "OpenRight";

    private int activeElementIndex = -1;
    private Vector3[] originalPositions;
    private bool isUIVisible = false;

    private Vector3 arrowInitialPosition;
    private bool arrowActive = false;

    void Start()
    {
        originalPositions = new Vector3[elements.Count];

        if (arrow != null)
        {
            arrow.SetActive(false);
            arrowInitialPosition = arrow.transform.position;
        }

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].pressEObject)
            {
                originalPositions[i] = elements[i].pressEObject.transform.localPosition;
                elements[i].pressEObject.SetActive(false);
            }

            if (elements[i].uiPanel)
                elements[i].uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < elements.Count; i++)
        {
            var el = elements[i];
            if (el.targetObject == null || el.pressEObject == null) continue;

            float distance = Vector3.Distance(player.transform.position, el.targetObject.transform.position);
            bool inRange = distance <= interactionDistance && !isUIVisible;

            el.pressEObject.SetActive(inRange);

            if (inRange)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }

                float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
                el.pressEObject.transform.localPosition = originalPositions[i] + new Vector3(0, offsetY, 0);
            }
        }

        // Handle E key press
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isUIVisible && closestIndex != -1)
            {
                ShowUI(closestIndex);

                // Show arrow and animate
                if (arrow != null && !arrowActive)
                {
                    arrow.SetActive(true);
                    arrowActive = true;
                }

                // Trigger door animations
                if (leftDoorAnimator != null)
                    leftDoorAnimator.SetTrigger(leftDoorTrigger);
                if (rightDoorAnimator != null)
                    rightDoorAnimator.SetTrigger(rightDoorTrigger);
            }
            else if (isUIVisible)
            {
                HideUI();
            }
        }

        // Auto-hide UI if player walks away
        if (isUIVisible && activeElementIndex != -1)
        {
            var el = elements[activeElementIndex];
            float dist = Vector3.Distance(player.transform.position, el.targetObject.transform.position);
            if (dist > interactionDistance)
            {
                HideUI();
            }
        }

        // Arrow animation
        if (arrow != null && arrow.activeSelf)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, arrow.transform.position);

            // Animate float
            float offsetY = Mathf.Sin(Time.time * arrowFloatFrequency) * arrowFloatAmplitude;
            Vector3 floatPosition = arrowInitialPosition + new Vector3(0, offsetY, 0);
            arrow.transform.position = floatPosition;

            // Rotate
            arrow.transform.Rotate(Vector3.up * arrowRotationSpeed * Time.deltaTime, Space.World);

            // Disable arrow if near
            if (distanceToPlayer <= arrowDisableDistance)
            {
                arrow.SetActive(false);
                arrowActive = false;
            }
        }
    }

    void ShowUI(int index)
    {
        if (index < 0 || index >= elements.Count) return;

        activeElementIndex = index;
        isUIVisible = true;

        var el = elements[activeElementIndex];
        if (el.uiPanel)
            el.uiPanel.SetActive(true);
    }

    void HideUI()
    {
        if (activeElementIndex < 0 || activeElementIndex >= elements.Count) return;

        var el = elements[activeElementIndex];
        if (el.uiPanel)
            el.uiPanel.SetActive(false);

        isUIVisible = false;
        activeElementIndex = -1;
    }
}
