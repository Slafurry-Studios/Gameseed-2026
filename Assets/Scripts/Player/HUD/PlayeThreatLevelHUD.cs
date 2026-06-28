using UnityEngine;
using UnityEngine.UI;

public class PlayerThreatLevelHUD : Singleton<PlayerThreatLevelHUD>
{
    [Header("UI References")]
    [SerializeField] private Slider threatSlider;

    public void UpdateThreat(float maxThreat, float currentThreat)
    {
        threatSlider.value = currentThreat / maxThreat;
    }
}