using UnityEngine;
using TMPro;

public class PlayerSubsCounterHUD : Singleton<PlayerSubsCounterHUD>
{
    [SerializeField] private TextMeshProUGUI subsText;

    public void UpdateSubscriber(float amount)
    {
        subsText.text = "Subsciber: " + amount;
    }
}