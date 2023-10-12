using System;
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

        private UiPopup _popup;
        private string _popupName = string.Empty;

        public void SetInfo(IconButtonInfo info)
        {
            nameText.text = info.name;
            icon.sprite = info.icon;
            background.sprite = info.background;
            _popupName = info.popupUiName;
            button.onClick.AddListener(ShowPopupUi);
        }

        private void ShowPopupUi()
        {
            if (_popup != null && _popup.gameObject.activeInHierarchy)
            {
                return;
            }

            if (string.IsNullOrEmpty(_popupName))
            {
                return;
            }

            _popup = UiManager.ShowPopupByName(_popupName);
        }

        protected override void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}