using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{

    bool count = false;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("port") && !count)
        {
            Debug.Log("Hello from Character Controller!");
            SceneManager.LoadScene("DemoScene");
            count = true;
        }
    }
}
