using System;
using Model.Item;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUi : UiPopup, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    [field: SerializeField] public Sprite SpriteImage { get; private set; }
    [field: SerializeField] public Image BackgroundImage { get; private set; }
    [field: SerializeField] public Image ItemImage { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
    [SerializeField] private Color selectedColor = Color.blue;

    public Item ItemData;
    public Action<InventoryItemUi, Vector2> OnItemSelected;
    public Action<Vector2> OnMoveItemEvent;
    public Action<InventoryItemUi, Vector2> OnDropItemEvent;

    public void Initialize(Item item)
    {
        ItemData = item;
    }

    private void SetSelected(bool selected)
    {
        if (selected)
        {
            BackgroundImage.color = selectedColor;
            ItemImage.color = selectedColor;
            transform.SetAsLastSibling();
        }
        else
        {
            BackgroundImage.color = Color.white;
            ItemImage.color = Color.white;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetSelected(true);
        OnItemSelected?.Invoke(this,eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SetSelected(false);
        OnDropItemEvent?.Invoke(this,eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //todo canvas caching
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
        Vector2 mousePosition = eventData.position;
        OnMoveItemEvent?.Invoke(mousePosition);
    }
}