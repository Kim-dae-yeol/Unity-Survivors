using UnityEngine;

namespace UI.InventoryUi
{
    public class InventorySlotUi : UiPopup
    {
        [field: SerializeField] public RectTransform ImageTransform { get; private set; }
    }
}