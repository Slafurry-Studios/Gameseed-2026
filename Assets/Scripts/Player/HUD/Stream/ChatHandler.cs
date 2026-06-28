using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.UI.HUD
{
    public class ChatHandler : MonoBehaviour
    {
        private TextMeshProUGUI user;
        private TextMeshProUGUI comments;

        private void Awake()
        {
            user = transform.Find("User").GetComponent<TextMeshProUGUI>();
            comments = transform.Find("Comments").GetComponent<TextMeshProUGUI>();

        }

        public void SetChat(string user, string comments)
        {
            this.user.text = user + ":";
            this.comments.text = comments;
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}