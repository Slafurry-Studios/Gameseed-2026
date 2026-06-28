using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI.HUD
{
    public class PlayerObjectiveItem : MonoBehaviour
    {
        [SerializeField] private Sprite objectiveCompleted;
        [SerializeField] private Sprite objectiveUncompleted;
        [SerializeField] private Image checkboxImage;
        [SerializeField] private TextMeshProUGUI objectiveText;

        private ObjectiveProgress progress;

        public void Setup(ObjectiveProgress data)
        {
            progress = data;
            checkboxImage.sprite = objectiveUncompleted;
            RefreshText();
        }

        public void TaskCompleted()
        {
            checkboxImage.sprite = objectiveCompleted;
            RefreshText();
        }

        public void UpdateStatus(string stat)
        {
            objectiveText.text = stat;
        }

        public void RefreshText()
        {
            if (progress == null) return;

            Transform nearest = ObjectiveTargetRegistry.GetNearest(
                progress.Data.Channel,
                Camera.main.transform.position
            );

            string nearestInfo = nearest != null
                ? $" | Nearest: {nearest.name} ({Vector3.Distance(Camera.main.transform.position, nearest.position):F0}m)"
                : " | No target found";

            objectiveText.text = $"{progress.Data.DisplayName} ({progress.CurrentValue}/{progress.Data.ObjectiveThreshold}){nearestInfo}";
        }

        public ObjectiveProgress GetProgress() => progress;
    }
}