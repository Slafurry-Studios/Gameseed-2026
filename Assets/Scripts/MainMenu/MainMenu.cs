using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private string levelScene;

    [Header("Main Menu UI")]
    [SerializeField] private GameObject guideCanvas;
    [SerializeField] private GameObject SettingCanvas;

    private void Start()
    {
        if (MusicManager.Instance != null){MusicManager.Instance.PlayMusic("MainMenu");}
        if (SettingCanvas != null){SettingCanvas.SetActive(false);}
    }

    public void StartGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        SceneManager.LoadScene(levelScene);
    }

    public void OpenSetting()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        if (SettingCanvas != null)
        {
            SettingCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Setting Canvas belum di-assign.");
        }
    }

    public void OpenGuide()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        if (guideCanvas != null)
            guideCanvas.SetActive(true);
        else
            Debug.LogWarning("Guide Canvas belum di-assign.");
    }

    // Closed
    public void CloseSetting()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        if (SettingCanvas != null)
        {
            SettingCanvas.SetActive(false);
        }
    }

    public void CloseGuide()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        if (guideCanvas != null)
            guideCanvas.SetActive(false);
    }

    public void ExitGame()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound2D("Button");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}