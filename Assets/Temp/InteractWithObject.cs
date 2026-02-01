using System.Collections.Generic;
using UnityEngine;

public class InteractWithObject : MonoBehaviour
{
    [Header("Player Reference")]
    public GameObject player;

    [Header("Press E Targets")]
    public List<GameObject> pressETargetObjects;   // Parent object to check distance
    public List<GameObject> pressEObjects;         // Floating prompt (child object)

    [Header("UI Panel")]
    public GameObject uiPanel;

    [Header("Glow Targets")]
    public List<GameObject> glowTargetObjects;     // Parent objects
    public List<GameObject> glowChildObjects;      // Outline child objects

    [Header("Distance Settings")]
    public float pressEDistance = 3f;
    public float glowDistance = 3f;

    [Header("Floating Animation Settings (Press E)")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    [Header("Glow Animation Settings")]
    public float scaleAmplitude = 0.05f;
    public float scaleFrequency = 1.5f;

    private bool isUIVisible = false;
    private Vector3[] originalPressEPositions;
    private Vector3[] originalGlowScales;

    void Start()
    {
        if (uiPanel) uiPanel.SetActive(false);

        // Save original positions/scales
        originalPressEPositions = new Vector3[pressEObjects.Count];
        for (int i = 0; i < pressEObjects.Count; i++)
        {
            if (pressEObjects[i])
            {
                pressEObjects[i].SetActive(false);
                originalPressEPositions[i] = pressEObjects[i].transform.localPosition;
            }
        }

        originalGlowScales = new Vector3[glowChildObjects.Count];
        for (int i = 0; i < glowChildObjects.Count; i++)
        {
            if (glowChildObjects[i])
            {
                glowChildObjects[i].SetActive(false);
                originalGlowScales[i] = glowChildObjects[i].transform.localScale;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        HandlePressEObjects();
        HandleGlowObjects();

        if (Input.GetKeyDown(KeyCode.E) && AnyPromptVisible())
        {
            isUIVisible = !isUIVisible;
            if (uiPanel) uiPanel.SetActive(isUIVisible);
            TogglePlayerControl(!isUIVisible);
        }

        if (!AnyPromptVisible() && isUIVisible)
        {
            isUIVisible = false;
            if (uiPanel) uiPanel.SetActive(false);
            TogglePlayerControl(true);
        }
    }

    void HandlePressEObjects()
    {
        for (int i = 0; i < pressETargetObjects.Count; i++)
        {
            if (pressETargetObjects[i] == null || pressEObjects[i] == null) continue;

            float distance = Vector3.Distance(player.transform.position, pressETargetObjects[i].transform.position);
            bool shouldShow = distance <= pressEDistance && !isUIVisible;

            pressEObjects[i].SetActive(shouldShow);

            if (shouldShow)
            {
                // Animate floating
                Vector3 startPos = originalPressEPositions[i];
                float offsetY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
                pressEObjects[i].transform.localPosition = new Vector3(startPos.x, startPos.y + offsetY, startPos.z);
            }
        }
    }

    void HandleGlowObjects()
    {
        for (int i = 0; i < glowTargetObjects.Count; i++)
        {
            if (glowTargetObjects[i] == null || glowChildObjects[i] == null) continue;

            float distance = Vector3.Distance(player.transform.position, glowTargetObjects[i].transform.position);
            bool shouldGlow = distance <= glowDistance;

            glowChildObjects[i].SetActive(shouldGlow);

            if (shouldGlow)
            {
                // Animate pulsing
                float scaleOffset = Mathf.Sin(Time.time * scaleFrequency) * scaleAmplitude;
                Vector3 baseScale = originalGlowScales[i];
                glowChildObjects[i].transform.localScale = baseScale + new Vector3(scaleOffset, scaleOffset, scaleOffset);
            }
        }
    }

    bool AnyPromptVisible()
    {
        foreach (var prompt in pressEObjects)
        {
            if (prompt != null && prompt.activeSelf)
                return true;
        }
        return false;
    }

    void TogglePlayerControl(bool enableMovement)
    {
        // You must plug in your own movement system logic here
        // Example: player.GetComponent<PlayerController>().enabled = enableMovement;
    }
}
