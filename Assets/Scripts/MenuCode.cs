using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCode : MonoBehaviour
{
    public GameObject MenuUI;
    public MonoBehaviour cameraScript;   // Assign your camera controller script here
    public MonoBehaviour playerMovementScript; // Assign your player movement script here

    private bool isMenuActive = false;

    void Start()
    {
        MenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraScript.enabled = true;
        playerMovementScript.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMenuActive = !isMenuActive;
            MenuUI.SetActive(isMenuActive);

            Cursor.lockState = isMenuActive ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isMenuActive;

            cameraScript.enabled = !isMenuActive;
            playerMovementScript.enabled = !isMenuActive;
        }
    }

    public void ResumeButton()
    {
         MenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraScript.enabled = true;
        playerMovementScript.enabled = true;
    }
    public void changeAvatar()
    {
        SceneManager.LoadScene("Select Avatar");
    }
    public void ExitButton()
    {
        SceneManager.LoadScene("Instruction");
    }

}
