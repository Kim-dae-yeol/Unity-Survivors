using System;
using Model.Item;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUi : UiPopup
{
    public InventoryItem ItemState;
    [field: SerializeField] public Sprite SpriteImage { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    [SerializeField] private Color selectedColor = Color.blue;
    [SerializeField] private Button button;

    public Action OnClickEvent;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        OnClickEvent?.Invoke();
    }

    public void UpdateUi(InventoryItem item)
    {
        
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            Image.color = selectedColor;
            transform.SetAsLastSibling();
        }
        else
        {
            Image.color = Color.white;
        }
    }
}