using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ChangeScene : MonoBehaviour
{
    public Toggle girlToggle;
    public Toggle boyToggle;
    public Button startButton;
    public TMP_Text startButtonText;
    public TMP_InputField nameInputField; //⬅️ Input field for name

    public static string selectedCharacter;
    public static string playerName; // ⬅️ Store name across scenes

    private string originalButtonText;

    void Start()
    {
        originalButtonText = startButtonText.text;

        girlToggle.onValueChanged.AddListener(OnGirlToggleChanged);
        boyToggle.onValueChanged.AddListener(OnBoyToggleChanged);
    }

    void OnGirlToggleChanged(bool isOn)
    {
        if (isOn) boyToggle.isOn = false;
    }

    void OnBoyToggleChanged(bool isOn)
    {
        if (isOn) girlToggle.isOn = false;
    }

    public void OnStartButtonClick()
    {
        playerName = nameInputField.text.Trim(); // ⬅️ Store the name

        if (string.IsNullOrEmpty(playerName))
        {
            StartCoroutine(ShowSelectAvatarWarning("Enter Name"));
            return;
        }

        if (girlToggle.isOn)
        {
            selectedCharacter = "GIRL1";
            SceneManager.LoadScene("Playground");
        }
        else if (boyToggle.isOn)
        {
            selectedCharacter = "BOY1";
            SceneManager.LoadScene("Playground");
        }
        else
        {
            StartCoroutine(ShowSelectAvatarWarning("Select Avatar"));
        }
    }

    private IEnumerator ShowSelectAvatarWarning(string warningText)
    {
        startButtonText.text = warningText;
        startButton.interactable = false;
        yield return new WaitForSeconds(3f);
        startButtonText.text = originalButtonText;
        startButton.interactable = true;
    }
}
