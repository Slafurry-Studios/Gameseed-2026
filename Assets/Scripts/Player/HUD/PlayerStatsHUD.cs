using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsHUD : Singleton<PlayerStatsHUD>
{
    [Header("UI References")]
    [SerializeField] private Image HealthUI;
    [SerializeField] private Image Health2UI;
    [SerializeField] private Image SnakeFaceUI;
    [SerializeField] private Slider EnergySlider;

    [Header("Assets")]
    [SerializeField] private Sprite[] HealthAsset = new Sprite[5];
    [SerializeField] private Sprite[] Health2Asset = new Sprite[5];
    [SerializeField] private Sprite[] SnakeFaceAsset = new Sprite[2];

    // public void 
}