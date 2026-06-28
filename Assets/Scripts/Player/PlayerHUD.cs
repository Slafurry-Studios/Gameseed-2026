using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game.Player; 

public class PlayerHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image healthFill;
    [SerializeField] private Image staminaFill;

    [Header("Data References")]
    [SerializeField] private PlayerMovement playerMovement; 
    [SerializeField] private PlayerHealth playerHealth;     

    [Header("Animation Settings")]
    [SerializeField] private float healthLerpSpeed = 5f;

   
    private void OnEnable()
    {
        
        if (playerMovement != null)
        {
            playerMovement.OnStaminaPctChanged += UpdateStaminaBar;
        }

        
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += HandleHealthChanged;
            playerHealth.OnDied += HandlePlayerDied;
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            playerMovement.OnStaminaPctChanged -= UpdateStaminaBar;
        }

        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
            playerHealth.OnDied -= HandlePlayerDied;
        }
    }

    private void HandlePlayerDied()
    {
        HandleHealthChanged(0f);
    }

    private void Start()
    {
        
        if (playerMovement != null)
        {
            UpdateStaminaBar(playerMovement.GetStaminaNormalized());
        }
        
       
        if (playerHealth != null) 
        {
            HandleHealthChanged(playerHealth.GetHealth());
        }
    }


    private void UpdateStaminaBar(float percentage)
    {
        if (staminaFill != null)
        {
            staminaFill.fillAmount = percentage;
        }
    }


    private void HandleHealthChanged(float percentage)
    {
        if (healthFill == null) return;

        StopAllCoroutines(); 
        StartCoroutine(SmoothLerpHealth(percentage));
    }

    private IEnumerator SmoothLerpHealth(float targetPercentage)
    {
        while (Mathf.Abs(healthFill.fillAmount - targetPercentage) > 0.01f)
        {
            healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, targetPercentage, Time.deltaTime * healthLerpSpeed);
            yield return null;
        }
        healthFill.fillAmount = targetPercentage; 
    }
}