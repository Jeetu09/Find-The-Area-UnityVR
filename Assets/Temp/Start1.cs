using UnityEngine;

public class Start1 : MonoBehaviour
{
    
    [Header("UI Canvas to Hide")]
    public GameObject uiCanvas;

    [Header("Player Armature or Controller")]
    public GameObject armature;

    private bool isUIActive = true;

    void Start()
    {
       
        if (uiCanvas != null)
            uiCanvas.SetActive(true);
        else
            Debug.LogWarning("UI Canvas is not assigned.");

        if (armature != null)
            armature.SetActive(false);
        else
            Debug.LogWarning("Armature (player) is not assigned.");
    }

    void Update()
    {
        if (isUIActive && Input.GetKeyDown(KeyCode.E))
        {
            
            if (uiCanvas != null)
                uiCanvas.SetActive(false);

            if (armature != null)
                armature.SetActive(true); // Movement script already enabled?

            isUIActive = false;
        }
    }
}
