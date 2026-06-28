using System;
using Game.Manager;
using Game.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.HUD
{

    public class UpgradeCardHUD : MonoBehaviour
    {
        private UpgradeCard upgradeCard;
        [SerializeField] private Image cardBg;
        [SerializeField] private Image cardIcon;
        [SerializeField] private TextMeshProUGUI cardTitle;
        [SerializeField] private TextMeshProUGUI cardDesc;
        [SerializeField] private Button cardButton;

        private void OnEnable()
        {
            if (cardButton != null)
                cardButton.onClick.AddListener(SetOnClick);
        }

        private void OnDisable()
        {
            if (cardButton != null)
                cardButton.onClick.RemoveListener(SetOnClick);
        }

        public void SetUpgradeCard(UpgradeCard upgradeCard)
        {
            this.upgradeCard = upgradeCard;
            SetUI();
            SetOnClick();
        }

        private void SetUI()
        {
            cardBg.sprite = upgradeCard.UpgradeCardData.UpgradeBg;
            cardIcon.sprite = upgradeCard.UpgradeCardData.UpgradeIcon;

            cardTitle.text = upgradeCard.UpgradeCardData.UpgradeName;

            string builder = upgradeCard.UpgradeCardData.UpgradeDesc[0];

            foreach (string descLine in upgradeCard.UpgradeCardData.UpgradeDesc)
            {
                if (descLine == upgradeCard.UpgradeCardData.UpgradeDesc[0]) continue;

                builder += "\n" + descLine;
            }
        }

        private void SetOnClick()
        {
            upgradeCard?.OnSelected();
            GetComponentInParent<UpgradeHUD>().Show(false);
        }
    }
}