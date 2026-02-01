using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class homeToInstruction : MonoBehaviour
{
  public void ChangeScene()
    {
        SceneManager.LoadScene("Instruction");
    }
    public void ChangeSceneToCharPage()
    {
        SceneManager.LoadScene("Select Avatar");
    }

    public InputField nameInputField;

    void Start()
    {
        // Enforce 15-character limit on the input field
        if (nameInputField != null)
        {
            nameInputField.characterLimit = 15;
        }
        else
        {
            Debug.LogWarning("InputField is not assigned in the Inspector.");
        }
    }
}
