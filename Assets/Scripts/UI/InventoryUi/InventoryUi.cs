using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUi
{
    public class InventoryUi : UiPopup,IDragHandler,IBeginDragHandler,IEndDragHandler
    {
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(CloseInventory);
        }

        private void CloseInventory()
        {
            transform.root.gameObject.SetActive(false);
        }
        public void OnDrag(PointerEventData eventData)
        {
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}