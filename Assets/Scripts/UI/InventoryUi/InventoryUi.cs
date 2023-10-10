using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.InventoryUi
{
    public class InventoryUi : UiPopup, IPointerMoveHandler
    {
        public Action<Vector2> OnMovePointer;
        private InventorySlotLayoutUi _slotLayoutUi;
        protected override void Awake()
        {
            base.Awake();
            _slotLayoutUi = GetComponentInChildren<InventorySlotLayoutUi>();
        }

        private void Start()
        {
            OnMovePointer += _slotLayoutUi.OnPointerMove;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            OnMovePointer?.Invoke(eventData.position);
        }
    }
}