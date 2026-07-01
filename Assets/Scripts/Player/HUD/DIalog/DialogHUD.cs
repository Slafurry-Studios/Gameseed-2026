using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Dialog;

namespace Game.UI.HUD
{
    public class DialogHUD : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image faceShot;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private GameObject nextDialogClue;
        [SerializeField] private GameObject dialogUIPrefab;

        [Header("Type Effect")]
        [SerializeField] private float typingSpeed = 0.03f;

        private DialogManager dialogManager;

        private bool isLast;
        private bool isTyping;

        private string currentDialog;
        private Coroutine typingCoroutine;

        private void Start()
        {
            dialogManager = FindAnyObjectByType<DialogManager>();
        }

        public void Show()
        {
            Time.timeScale = 0f;
            dialogUIPrefab.SetActive(true);
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            dialogUIPrefab.SetActive(false);
        }

        public void SetDialog(Sprite faceShotSprite, string characterName, string dialog, bool isLast)
        {
            faceShot.sprite = faceShotSprite;
            nameText.text = characterName;

            currentDialog = dialog;
            this.isLast = isLast;

            nextDialogClue.SetActive(false);

            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeDialog());
        }

        private IEnumerator TypeDialog()
        {
            isTyping = true;
            dialogText.text = "";

            foreach (char c in currentDialog)
            {
                dialogText.text += c;
                yield return new WaitForSecondsRealtime(typingSpeed);
            }

            dialogText.text = currentDialog;
            isTyping = false;

            if (!isLast)
                nextDialogClue.SetActive(true);
        }

        public void NextDialog()
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);

                dialogText.text = currentDialog;
                isTyping = false;

                if (!isLast)
                    nextDialogClue.SetActive(true);

                return;
            }

            if (isLast)
            {
                Hide();
                isLast = false;
                return;
            }

            dialogManager.NextDialog();
        }
    }
}