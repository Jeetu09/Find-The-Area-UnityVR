using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class starttoavatar : MonoBehaviour
{
    // Start is called before the first frame update
   public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("Select Avatar");

    }
}
