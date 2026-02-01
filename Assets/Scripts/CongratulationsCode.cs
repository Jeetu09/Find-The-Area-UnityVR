
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CongratulationsCode : MonoBehaviour
{
    public GameObject CongoUI;
    public MonoBehaviour cameraScript;           // Assign your camera controller script here
    public MonoBehaviour playerMovementScript;   // Assign your player movement script here
    public TMP_Text nameText;                    // Assign this in the inspector

    public GameObject BoyAvatarPic;              // Assign Boy avatar image in inspector
    public GameObject GirlAvatarPic;             // Assign Girl avatar image in inspector

    void Start()
    {
        CongoUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraScript.enabled = true;
        playerMovementScript.enabled = true;

        // Set player name
        if (nameText != null)
        {
            if (!string.IsNullOrEmpty(ChangeScene.playerName))
                nameText.text = ChangeScene.playerName;
            else
                nameText.text = "Player";
        }
        else
        {
            Debug.LogWarning("NameText is not assigned in the inspector.");
        }

        // Show avatar based on selected character
        if (ChangeScene.selectedCharacter == "BOY1")
        {
            BoyAvatarPic.SetActive(true);
            GirlAvatarPic.SetActive(false);
        }
        else if (ChangeScene.selectedCharacter == "GIRL1")
        {
            BoyAvatarPic.SetActive(false);
            GirlAvatarPic.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No character selected, showing default Boy avatar.");
            BoyAvatarPic.SetActive(true);
            GirlAvatarPic.SetActive(false);
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Playground");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("Instruction");
    }
}
