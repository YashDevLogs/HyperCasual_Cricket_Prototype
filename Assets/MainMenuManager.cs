using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For UI elements

public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsPanel; // Reference to the settings panel
    public Button startButton; // Reference to the Start button
    public Button settingsButton; // Reference to the Settings button
    public Button quitButton; // Reference to the Quit button

    void Start()
    {
        // Add listeners to buttons
        startButton.onClick.AddListener(OnStartButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);

        // Ensure settings panel is inactive at the start
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    void OnStartButtonClicked()
    {
        // Load the scene named "SampleScene"
        SceneManager.LoadScene("SampleScene");
    }

    void OnSettingsButtonClicked()
    {
        // Toggle the settings panel visibility
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    void OnQuitButtonClicked()
    {
        // Quit the application
        Application.Quit();
    }
}
