using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.HUD
{
    public class PlayerThreatLevelHUD : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider threatSlider;

        public void UpdateThreat(float maxThreat, float currentThreat)
        {
            threatSlider.value = currentThreat / maxThreat;
        }
    }
}