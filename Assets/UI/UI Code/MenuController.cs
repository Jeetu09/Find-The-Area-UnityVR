using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;                 // UI panel to show/hide
    public InputActionReference openMenuAction;  // Action to open/close menu

    void Start()
    {
        menuPanel.SetActive(false);
    }

    private void Awake()
    {
        // Enable the input action
        openMenuAction.action.Enable();
        openMenuAction.action.performed += ToggleMenu;

        // Listen for device connection/disconnection
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy()
    {
        // Clean up when destroyed
        openMenuAction.action.Disable();
        openMenuAction.action.performed -= ToggleMenu;

        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        // Toggle panel visibility
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                openMenuAction.action.Disable();
                openMenuAction.action.performed -= ToggleMenu;
                break;

            case InputDeviceChange.Reconnected:
                openMenuAction.action.Enable();
                openMenuAction.action.performed += ToggleMenu;
                break;
        }
    }

    public void EndGame()
    {
        // If running inside the Unity Editor, stop playing
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running as a built game, quit the application
        Application.Quit();
#endif
    }

    public void RestartGame()
    {
        // Reloads the scene that is currently active
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

      

}
