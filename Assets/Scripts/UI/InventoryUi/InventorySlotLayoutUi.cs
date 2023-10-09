using System;
using System.Collections.Generic;
using UI;
using UI.InventoryUi;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class InventorySlotLayoutUi : UiPopup
{
    [SerializeField] private int cellCount;
    [SerializeField] private RectTransform items;
    private GridLayoutGroup _gridLayout;
    private List<InventorySlotUi> _slots = new List<InventorySlotUi>();

    //todo item info modeling
    private int _itemWidth = 1;
    private int _itemHeight = 1;
    private Vector2Int _startPosition = Vector2Int.zero;

    protected override void Awake()
    {
        base.Awake();
        _gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        for (int i = 0; i < cellCount; i++)
        {
            InventorySlotUi slot = UiManager.ShowPopupByName(nameof(InventorySlotUi)).GetComponent<InventorySlotUi>();
            slot.transform.SetParent(_gridLayout.transform, false);
            _slots.Add(slot.GetComponent<InventorySlotUi>());
        }

        CreateItems();
    }

    private void CreateItems()
    {
        //todo createItem at position
        //todo foreach loop
        UiPopup popup = UiManager.ShowPopupByName(nameof(InventoryItemUi));
        InventoryItemUi item = popup.GetComponentInChildren<InventoryItemUi>();
        Transform itemTransform = item.transform;
        itemTransform.SetParent(items, false);
        Vector2 itemPosition = GetItemPosition(item.Row, item.Col, _itemWidth, _itemHeight);
        Vector3 scale = itemTransform.localScale;
        scale.x *= _itemWidth;
        scale.y *= _itemHeight;
        itemTransform.localScale = scale;

        itemTransform.localPosition = itemPosition;
    }

    //아이템 생성, 위치 변경시 호출할 함수
    private Vector2 GetItemPosition(int row, int col, int width, int height)
    {
        //todo row, col -> getPosition
        // todo 
        Vector2 cellSize = _gridLayout.cellSize;
        float xPos = cellSize.x * col * width - cellSize.x * 0.5f;
        float yPos = -cellSize.y * row * height + cellSize.y * 0.5f;
        //position of items is pivot
        return new Vector2(xPos, yPos);
    }

    //drag-drop 으로 아이템의 위치 변경시 호출
    private void ChangePosition()
    {
    }

    //drag-drop 상태에서 hover 이벤트 시에 호출
    public void ShowHoverBlocks()
    {
    }
}