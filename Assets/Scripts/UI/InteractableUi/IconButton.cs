using Model.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.InteractableUi
{
    using Text = TextMeshProUGUI;

    public class IconButton : UiPopup
    {
        public Text nameText;
        public Button button;
        public Image icon;
        public Image background;

        private string _popupName = string.Empty;

        public void SetInfo(IconButtonInfo info)
        {
            nameText.text = info.name;
            icon.sprite = info.icon;
            background.sprite = info.background;
            button.onClick.AddListener(ShowPopupUi);
            _popupName = info.popupUiName;
        }

        private void ShowPopupUi()
        {
            if (string.IsNullOrEmpty(_popupName))
            {
                return;
            }

            UiManager.ShowPopupByName(_popupName);
        }
    }
}