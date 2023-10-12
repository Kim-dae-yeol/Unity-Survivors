using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUi
{
    public class InventoryUi : UiPopup, IDragHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Button closeButton;
        [SerializeField] private RectTransform rectTransform;

        private void Start()
        {
            closeButton.onClick.AddListener(CloseInventory);
        }

        private void CloseInventory()
        {
            gameObject.SetActive(false);
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}