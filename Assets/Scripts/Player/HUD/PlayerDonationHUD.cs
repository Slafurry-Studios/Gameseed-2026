using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerDonationHUD : Singleton<PlayerDonationHUD>
{
    [SerializeField] private GameObject donationUI;
    [SerializeField] private TextMeshProUGUI donationText;
    [SerializeField] private RectTransform donationRect;
    [SerializeField] private CanvasGroup donationCanvasGroup;

    [Header("Settings")]
    [SerializeField] private float showDuration = 2.5f;
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.4f;
    [SerializeField] private float slideDistance = 30f;

    private Coroutine currentRoutine;
    private Vector2 originalPos;

    protected override void Awake()
    {
        base.Awake();
        if (donationRect == null) donationRect = donationUI.GetComponent<RectTransform>();
        if (donationCanvasGroup == null) donationCanvasGroup = donationUI.GetComponent<CanvasGroup>();
        originalPos = donationRect.anchoredPosition;
    }

    private void OnEnable()
    {
        ObjectiveManager.Instance.OnObjectiveAdded += HandleObjectiveAdded;
    }

    private void OnDisable()
    {
        ObjectiveManager.Instance.OnObjectiveAdded -= HandleObjectiveAdded;
    }

    private void HandleObjectiveAdded(Objective objective)
    {
        if (objective.Channel != null && objective.Channel.useDonation)
        {
            UpdateDonation(objective.Channel.donationMessage);
        }
    }

    public void UpdateDonation(string donation)
    {
        donationText.text = donation;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowDonationRoutine());
    }

    private IEnumerator ShowDonationRoutine()
    {
        donationUI.SetActive(true);

        Vector2 startPos = originalPos - new Vector2(0f, slideDistance);
        donationRect.anchoredPosition = startPos;
        donationCanvasGroup.alpha = 0f;

        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / fadeInDuration);
            float eased = 1f - Mathf.Pow(1f - p, 3f);

            donationCanvasGroup.alpha = eased;
            donationRect.anchoredPosition = Vector2.Lerp(startPos, originalPos, eased);

            yield return null;
        }

        donationCanvasGroup.alpha = 1f;
        donationRect.anchoredPosition = originalPos;

        yield return new WaitForSeconds(showDuration);

        t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / fadeOutDuration);
            donationCanvasGroup.alpha = 1f - p;
            yield return null;
        }

        donationCanvasGroup.alpha = 0f;
        donationUI.SetActive(false);

        currentRoutine = null;
    }
}