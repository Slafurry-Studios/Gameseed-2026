using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingPause;
    private bool isPaused = false;

    void Start()
    {
        if (pauseMenuUI != null) { pauseMenuUI.SetActive(false); }
        if (settingPause != null) { settingPause.SetActive(false); }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }
    public void PauseGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");
        if (pauseMenuUI != null) { pauseMenuUI.SetActive(true); }
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OpenSetting()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        if (settingPause != null)
        {
            settingPause.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Setting Canvas belum di-assign.");
        }
    }

    // Closed
    public void ResumeGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");
        if (pauseMenuUI != null) { pauseMenuUI.SetActive(false); }
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseSetting()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        if (settingPause != null)
        {
            settingPause.SetActive(false);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        if (string.IsNullOrEmpty(mainMenuSceneName))
        {
            Debug.LogWarning("Main menu scene name belum diisi.");
            return;
        }

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.LoadScene(mainMenuSceneName);
        else
            SceneManager.LoadScene(mainMenuSceneName);
    }
}
