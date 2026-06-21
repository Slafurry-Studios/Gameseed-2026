using UnityEngine;
using UnityEngine.UI;
using Game.Player; // Required to access your PlayerMovement script

public class PlayerHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image staminaFill;

    [Header("Data References")]
    [SerializeField] private PlayerMovement playerMovement;

    private void OnEnable()
    {
        if (playerMovement != null)
        {
            // Subscribe to the event
            playerMovement.OnStaminaPctChanged += UpdateStaminaBar;
        }
    }

    private void OnDisable()
    {
        if (playerMovement != null)
        {
            // Always unsubscribe to prevent memory leaks
            playerMovement.OnStaminaPctChanged -= UpdateStaminaBar;
        }
    }

    private void Start()
    {
        // Fetch the initial state just in case the UI loads slightly after the player
        if (playerMovement != null)
        {
            UpdateStaminaBar(playerMovement.GetStaminaNormalized());
        }
    }

    private void UpdateStaminaBar(float percentage)
    {
        staminaFill.fillAmount = percentage;
    }
}