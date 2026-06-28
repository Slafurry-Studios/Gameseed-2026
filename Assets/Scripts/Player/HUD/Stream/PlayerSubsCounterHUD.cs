using UnityEngine;
using TMPro;

namespace Game.UI.HUD
{

    public class PlayerSubsCounterHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI subsText;

        public void UpdateSubscriber(float amount)
        {
            subsText.text = "Subsciber: " + amount;
        }
    }

}