using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Model.Item;
using UI;
using UI.InventoryUi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;


public class InventorySlotLayoutUi : UiPopup
{
    private struct HoverInfo
    {
        public int Row;
        public int Col;
    }

    [SerializeField] private int maxRows;
    [SerializeField] private int maxCols;
    [SerializeField] private RectTransform items;
    
    [SerializeField] private Color canPositionColor = Color.green;
    [SerializeField] private Color canNotPositionColor = Color.red;
    private int _cellCount;
    private GridLayoutGroup _gridLayout;
    private List<InventorySlotUi> _slots = new List<InventorySlotUi>();

    //uiState
    private InventoryItemUi _selectedItem;
    private Vector2 _screenPoint;
    private HoverInfo _hoverInfo;

    private InventoryManager _inventoryManager;

    protected override void Awake()
    {
        base.Awake();
        _gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        _inventoryManager = GameManager.Instance.InventoryManager;
        _cellCount = maxRows * maxCols;
        for (int i = 0; i < _cellCount; i++)
        {
            InventorySlotUi slot = UiManager.ShowPopupByName(nameof(InventorySlotUi)).GetComponent<InventorySlotUi>();
            slot.transform.SetParent(_gridLayout.transform, false);
            _slots.Add(slot.GetComponent<InventorySlotUi>());
        }

        CreateItems();
    }


    private void CreateItems()
    {
        foreach (var item in _inventoryManager.Items)
        {
            CreateItem(item);
        }
    }

    private void CreateItem(InventoryItem item)
    {
        //todo refactor this function -> divide into ItemUi
        UiPopup popup = UiManager.ShowPopupByName(nameof(InventoryItemUi));
        InventoryItemUi itemUi = popup.GetComponentInChildren<InventoryItemUi>();
        itemUi.OnClickEvent += () => { OnItemClicked(itemUi); };
        Transform itemTransform = itemUi.transform;
        itemTransform.SetParent(items, false);
        itemUi.ItemState = item;

        Vector2 itemPosition = GetItemLocalPosition(item.Row, item.Col, item.Item.Width, item.Item.Height);
        Vector3 scale = itemTransform.localScale;
        scale.x = item.Item.Width;
        scale.y = item.Item.Height;

        itemTransform.localScale = scale;
        itemTransform.localPosition = itemPosition;
    }

    private void OnItemClicked(InventoryItemUi item)
    {
        if (_selectedItem == item)
        {
            //where to position this function
            if (IsInInventoryWindow())
            {
                //todo canPosition-> position else position to prev
                int itemIndex = GetItemPositionIndex();
                int row = itemIndex / maxRows;
                int col = itemIndex % maxCols;
                if (CanPosition(row, col, _selectedItem.ItemState.Item.Width, _selectedItem.ItemState.Item.Height))
                {
                    //밑에 물건이 깔린 경우
                    if (IsCurrentSlotExists(row, col))
                    {
                        InventoryItemUi itemUi = _selectedItem;
                        InventoryItemUi pointerItem = GetInventoryItemUiAt(row, col);
                        if (pointerItem == null)
                        {
                            return;
                        }
                        _selectedItem = pointerItem;
                        ChangePosition(itemUi, row, col);
                    }
                    else
                    {
                        ChangePosition(_selectedItem, row, col);
                    }

                    HideHoveredBlocks();
                }
                else
                {
                    //todo 현재 물건을 
                }
            }
            else
            {
                _inventoryManager.RemoveItem(item.ItemState);
                item.gameObject.SetActive(false);
                _selectedItem = null;
            }

            return;
        }
        _selectedItem = item;
        item.SetSelected(true);
    }

    private Vector2Int GetInventoryIndexVector(Vector2 position)
    {
        Vector2 localPosition = position - (Vector2)transform.position;
        int x = (int)(localPosition.x / _gridLayout.cellSize.x);
        int y = (int)(-localPosition.y / _gridLayout.cellSize.y);
        return new Vector2Int(x, y);
    }

    //아이템 생성, 위치 변경시 호출할 함수
    private Vector2 GetItemLocalPosition(int row, int col, int width, int height)
    {
        //todo row, col -> getPosition
        // todo 
        Vector2 cellSize = _gridLayout.cellSize;
        float xPos = cellSize.x * col + cellSize.x * 0.5f * width;
        float yPos = -cellSize.y * row - cellSize.y * 0.5f * height;
        //position of items is pivot
        return new Vector2(xPos, yPos);
    }

    //drag-drop 으로 아이템의 위치 변경시 호출
    private void ChangePosition(InventoryItemUi inventoryItemUi, int row, int col)
    {
        _selectedItem = null;
        inventoryItemUi.SetSelected(false);
        inventoryItemUi.ItemState.Row = row;
        inventoryItemUi.ItemState.Col = col;
        Item item = inventoryItemUi.ItemState.Item;
        Vector2 position = GetItemLocalPosition(row, col, item.Width, item.Height);
        inventoryItemUi.transform.localPosition = position;
    }

    private void ShowHoveredBlocks()
    {
        if (_hoverInfo.Row == -1 || _hoverInfo.Col == -1)
        {
            return;
        }

        //todo extract inline function
        Item item = _selectedItem.ItemState.Item;
        int startIndex = GetItemPositionIndex();
        bool canPosition = CanPosition(
            row: _hoverInfo.Row,
            col: _hoverInfo.Col,
            width: _selectedItem.ItemState.Item.Width,
            height: _selectedItem.ItemState.Item.Height
        );
        Color color = canPosition ? canPositionColor : canNotPositionColor; // false

        for (int i = 0; i < item.Height; i++)
        {
            for (int j = 0; j < item.Width; j++)
            {
                int index = startIndex + i * maxCols + j;
                Image img = _slots[index].ImageTransform;
                img.color = color;
            }
        }
    }


    private void HideHoveredBlocks()
    {
        if (_hoverInfo.Row == -1 || _hoverInfo.Col == -1)
        {
            return;
        }

        if (_selectedItem == null)
        {
            return;
        }

        int startIndex = GetItemPositionIndex();
        Item item = _selectedItem.ItemState.Item;
        for (int i = 0; i < item.Height; i++)
        {
            for (int j = 0; j < item.Width; j++)
            {
                //todo caching data into InventorySlotUi
                int index = startIndex + i * maxCols + j;
                Image img = _slots[index].ImageTransform;
                img.color = Color.white;
            }
        }
    }

    private int GetItemPositionIndex()
    {
        Item item = _selectedItem.ItemState.Item;
        int row = _hoverInfo.Row;
        int col = _hoverInfo.Col;
        if ((row + item.Height - 1) >= maxRows)
        {
            // height, maxCols both are not zero based... -1 - (-1)
            row = _hoverInfo.Row - ((_hoverInfo.Row + item.Height) - maxRows);
        }

        if ((col + item.Width - 1) >= maxCols)
        {
            // width, maxCols both are not zero based... -1 - (-1)
            col = _hoverInfo.Col - ((_hoverInfo.Col + item.Width) - maxCols);
        }

        int startIndex = row * maxCols + col;
        return startIndex;
    }

    public void OnPointerMove(Vector2 pointer)
    {
        _screenPoint = pointer;
        if (_selectedItem == null)
        {
            return;
        }

        MoveSelectedItem(pointer);
        Vector2Int term = GetInventoryIndexVector(pointer);
        int row = term.y;
        int col = term.x;
        if (row >= maxRows || row < 0 || col >= maxCols || col < 0)
        {
            HideHoveredBlocks();
            return;
        }


        HideHoveredBlocks();
        _hoverInfo.Row = row;
        _hoverInfo.Col = col;
        ShowHoveredBlocks();
    }

    private void MoveSelectedItem(Vector2 position)
    {
        if (_selectedItem == null)
        {
            return;
        }

        _selectedItem.transform.position = position;
    }

    private bool CanPosition(int row, int col, int width, int height)
    {
        //todo
        return true;
    }

    private bool IsCurrentSlotExists(int row, int col)
    {
        return _inventoryManager.Items
            .Any(item => item.Row <= row && (item.Row + item.Item.Height) >= row && (item.Col) <= col &&
                         (item.Col + item.Item.Width) >= col);
    }

    private bool IsInInventoryWindow()
    {
        Vector2 position = transform.position;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        float width = rect.width;
        float height = rect.height;
        return _screenPoint.x >= position.x &&
               _screenPoint.x <= position.x + width &&
               _screenPoint.y <= position.y &&
               _screenPoint.y >= position.y - height;
    }

    public InventoryItemUi GetInventoryItemUiAt(int row, int col)
    {
        InventoryItemUi[] inventoryUiSet = items.GetComponentsInChildren<InventoryItemUi>();
        return inventoryUiSet.FirstOrDefault(ui => ui.ItemState.IsPositionInItem(row, col));
    }
}