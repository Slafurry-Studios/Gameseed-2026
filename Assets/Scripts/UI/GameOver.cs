using System.Collections;
using Game.Manager;
using Game.Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private string levelScene = "MainMenu";
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject respawnPos;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI threatPoint;
    [SerializeField] private TextMeshProUGUI growthPoint;
    [SerializeField] private TextMeshProUGUI SubscriberPoint;

    [Header("Player Data")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerCollection playerGrowth;
    [SerializeField] private ThreatPointManager playerThreath;
    [SerializeField] private SubscriptionPointManager playerSubs;
    // [SerializeField] private PlayerGrowth playerGrowth;

    [Header("Settings")]
    [SerializeField] private float freezeDuration = 2f;

    private bool gameOver = false;
    private float timerReset = 3f;
    private bool isCounting = false;
    public bool IsCounting => isCounting;
    private static bool shouldCountdown = false;

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

        if (shouldCountdown)
        {
            shouldCountdown = false;
            Time.timeScale = 0f;
            StartCoroutine(StartCountdown());
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
        yield return new WaitForSeconds(freezeDuration);

        if (threatPoint != null && playerThreath != null)
        {
            threatPoint.text = playerThreath.GetPoint() + " Threat Point";
        }

        if (SubscriberPoint != null && playerSubs != null)
        {
            SubscriberPoint.text = playerSubs.GetPoint() + " Subscription";
        }

        if (growthPoint != null && playerGrowth != null)
        {
            growthPoint.text = playerGrowth.GetOrbs() + " Growth";
        }

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private IEnumerator StartCountdown()
    {
        if (TransitionManager.Instance != null)
        {
            yield return new WaitForSecondsRealtime(0.2f);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }

        isCounting = true;
        Time.timeScale = 0f;

        float timer = timerReset;

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        while (timer > 0)
        {
            if (timerText != null)
            {
                timerText.text = Mathf.Ceil(timer).ToString();
            }

            timer -= Time.unscaledDeltaTime;
            yield return null;
        }

        if (timerText != null)
        {
            timerText.text = "GO!";
        }

        yield return new WaitForSecondsRealtime(1f);

        if (timerText != null)
        {
            timerText.text = "";
            timerText.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
        isCounting = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        gameOver = false;
        shouldCountdown = true;

        if (TransitionManager.Instance != null)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            TransitionManager.Instance.LoadScene(currentScene);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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