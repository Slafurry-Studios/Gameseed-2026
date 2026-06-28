using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsHUD : Singleton<PlayerStatsHUD>
{
    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;

    public void UpdateHealth(float maxHealth, float currentHealth)
    {
        healthSlider.value = currentHealth / maxHealth;
    }

    public void UpdateStamina(float maxStamina, float currentStamina)
    {
        staminaSlider.value = currentStamina / maxStamina;
    }
}