using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class help_button : MonoBehaviour
{
    [Header("UI Panel to Show")]
    public GameObject uiPanel; // Assign the UI Panel in Inspector

    [Header("Player Armature or Controller")]
    public GameObject armature; // Assign your player GameObject here (with movement script)

    private bool uiActive = false;

    void Start()
    {

        if (uiPanel != null)
            uiPanel.SetActive(false);
        else
            Debug.LogWarning("UI Panel not assigned in StartUI script.");

        if (armature != null)
            armature.SetActive(false); // Disable player at start
        else
            Debug.LogWarning("Armature not assigned in StartUI script.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (uiPanel != null)
                uiPanel.SetActive(true);

            if (armature != null)
                armature.SetActive(false); // Re-enable player

            uiActive = true;
        }
        if (uiActive && Input.GetKeyDown(KeyCode.E))
        {
            if (uiPanel != null)
                uiPanel.SetActive(false);

            if (armature != null)
                armature.SetActive(true); // Re-enable player

            uiActive = false;
        }
    }
}
