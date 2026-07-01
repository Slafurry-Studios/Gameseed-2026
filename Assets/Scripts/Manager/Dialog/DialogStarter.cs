using UnityEngine;

namespace Game.Dialog
{
    public class DialogStarter : MonoBehaviour
    {
        [SerializeField] private string dialogBucket;
        private DialogManager dialogManager;
        void OnEnable()
        {
            dialogManager = FindAnyObjectByType<DialogManager>();
            dialogManager.OnDialogHUDReady += (isReady) =>
            {
                if (isReady)
                {
                    StartDialog();
                }
            };
        }

        public void StartDialog()
        {
            if (dialogBucket != null)
            {
                dialogManager.StartDialog(dialogBucket);
            }
            else
            {
                Debug.LogWarning("Dialog is not assigned in DialogStarter.");
            }
        }
    }
}