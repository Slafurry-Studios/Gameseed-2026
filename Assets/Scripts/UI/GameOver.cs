using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Player;

public class GameOver : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private string levelScene = "MainMenu";
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Player Data")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Settings")]
    [SerializeField] private float freezeDuration = 2f;

    private bool gameOver = false;

    private void Start()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        if (playerHealth != null)
        {
            playerHealth.OnDied += HandlePlayerDied;
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDied -= HandlePlayerDied;
        }
    }

    private void HandlePlayerDied()
    {
        TriggerGameOver();
    }

    private void TriggerGameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        // Tunggu sampai durasi animasi dizzy selesai (default 2 detik)
        yield return new WaitForSeconds(freezeDuration);

        // Hentikan pergerakan waktu permainan
        Time.timeScale = 0f;

        // Tampilkan popup Game Over
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        if (string.IsNullOrEmpty(levelScene))
        {
            Debug.LogWarning("Level scene name belum diisi.");
            return;
        }

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.LoadScene(levelScene);
        else
            SceneManager.LoadScene(levelScene);
    }
}