using UnityEngine;
using TMPro;

public class CharacterManager : MonoBehaviour
{
    public GameObject boyCharacter;
    public GameObject girlCharacter;

    public Avatar boyAvatar;
    public Avatar girlAvatar;

    public Animator characterAnimator; // Shared animator reference

    public TMP_Text nameText; // Assign this in the inspector

    void Start()
    {
        GameObject selectedCharacter = null;

        // Activate selected character and assign avatar
        if (ChangeScene.selectedCharacter == "BOY1")
        {
            boyCharacter.SetActive(true);
            girlCharacter.SetActive(false);
            selectedCharacter = boyCharacter;
            characterAnimator.avatar = boyAvatar;
        }
        else if (ChangeScene.selectedCharacter == "GIRL1")
        {
            girlCharacter.SetActive(true);
            boyCharacter.SetActive(false);
            selectedCharacter = girlCharacter;
            characterAnimator.avatar = girlAvatar;
        }
        else
        {
            Debug.LogWarning("No character selected, enabling default.");
            boyCharacter.SetActive(true);
            girlCharacter.SetActive(false);
            selectedCharacter = boyCharacter;
            characterAnimator.avatar = boyAvatar;
        }

        // Set the name in the TMP_Text field
        if (nameText != null)
        {
            if (!string.IsNullOrEmpty(ChangeScene.playerName))
            {
                nameText.text = ChangeScene.playerName;
            }
            else
            {
                nameText.text = "Player";
            }
        }
        else
        {
            Debug.LogWarning("NameText is not assigned in the inspector.");
        }
    }
}
