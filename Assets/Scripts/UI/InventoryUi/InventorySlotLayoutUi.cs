using System.Linq;
using Managers;
using Model.Item;
using UI;
using UI.InventoryUi;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotLayoutUi : UiPopup
{
    [SerializeField] private int maxRows;
    [SerializeField] private int maxCols;
    [SerializeField] private RectTransform items;

    [SerializeField] private Color canPositionColor = Color.green;
    [SerializeField] private Color canNotPositionColor = Color.red;

    private GridLayoutGroup _gridLayout;
    private InventorySlotUi[,] _slots;
    private int[,] _itemIndexOnSlot;
    private bool _isChanged;


    //uiState
    private Item _selectedItemData;
    private Vector2 _screenPoint;
    private Vector2Int _hoverInfo;
    private Vector2Int _selectedItemStartIndex;

    private InventoryManager _inventoryManager;
    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = GameManager.Instance;
        _gridLayout = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        _inventoryManager = GameManager.Instance.InventoryManager;
        _slots = new InventorySlotUi[maxRows, maxCols];
        _itemIndexOnSlot = new int[maxRows, maxCols];

        for (int i = 0; i < maxRows; i++)
        {
            for (int j = 0; j < maxCols; j++)
            {
                InventorySlotUi slot = UiManager.ShowPopupByName(nameof(InventorySlotUi))
                    .GetComponent<InventorySlotUi>();
                slot.transform.SetParent(_gridLayout.transform, false);

                _slots[i, j] = slot.GetComponent<InventorySlotUi>();
                _itemIndexOnSlot[i, j] = -1;
            }
        }

        CreateItems();
    }

    private void CreateItems()
    {
        foreach ((InventoryItem item, int index) in _inventoryManager.Items.Select((item, i) => (item, i)))
        {
            CreateItem(item, index);
        }
    }

    private void CreateItem(InventoryItem item, int index)
    {
        for (int i = item.Row; i < item.Row + item.Item.Height; i++)
        {
            for (int j = item.Col; j < item.Col + item.Item.Width; j++)
            {
                _itemIndexOnSlot[i, j] = index;
            }
        }

        UiPopup popup = UiManager.ShowPopupByName(nameof(InventoryItemUi));
        InventoryItemUi itemUi = popup.GetComponentInChildren<InventoryItemUi>();
        itemUi.OnDropItemEvent += OnDropItem;
        itemUi.OnMoveItemEvent += OnItemMoved;
        itemUi.OnItemSelected += OnItemSelected;
        itemUi.Initialize(item.Item);
        Transform itemTransform = itemUi.transform;
        itemTransform.SetParent(items, false);

        Vector2 itemPosition = GetItemLocalPosition(item.Row, item.Col, item.Item.Width, item.Item.Height);
        Vector3 scale = itemTransform.localScale;
        scale.x = item.Item.Width;
        scale.y = item.Item.Height;

        itemTransform.localScale = scale;
        itemTransform.localPosition = itemPosition;
    }

    private void OnItemSelected(InventoryItemUi item, Vector2 screenPointer)
    {
        _selectedItemData = item.ItemData;
        //todo center Position...
        _selectedItemStartIndex = GetInventoryPositionAt(screenPointer);
        Debug.Log($"OnItemSelected-index is {_selectedItemStartIndex}");
    }

    private void OnItemMoved(Vector2 position)
    {
        //todo
        HideHoveredBlocks();
        UpdateHoverInfo(position);
        ShowHoveredBlocks();
    }

    private void UpdateHoverInfo(Vector2 position)
    {
        _hoverInfo = GetInventoryPositionAt(position);
    }

    private void OnDropItem(InventoryItemUi itemUi, Vector2 screenPointer)
    {
        Debug.Log($"dropItem - item start index is {_selectedItemStartIndex}");
        HideHoveredBlocks();

        //1. is in inventory Window
        if (IsInInventoryWindow(screenPointer))
        {
            if (CanPosition(_hoverInfo.y, _hoverInfo.x, _selectedItemData.Width, _selectedItemData.Height))
            {
                Vector2 position = GetItemLocalPosition(
                    row: _hoverInfo.y,
                    col: _hoverInfo.x,
                    width: _selectedItemData.Width,
                    height: _selectedItemData.Height);
                itemUi.transform.localPosition = position;

                ChangePosition();
                UpdateInventoryUiState();
                for (int i = 0; i < maxRows; i++)
                {
                    for (int j = 0; j < maxCols; j++)
                    {
                        Debug.Log($"({i}, {j}) : {_itemIndexOnSlot[i, j]}");
                    }
                }
            }
            else
            {
                //원위치
                Vector2 position = GetItemLocalPosition(
                    row: _selectedItemStartIndex.y,
                    col: _selectedItemStartIndex.x,
                    width: _selectedItemData.Width,
                    height: _selectedItemData.Height);
                itemUi.transform.localPosition = position;
                _selectedItemData = null;
                _selectedItemStartIndex.x = -1;
                _selectedItemStartIndex.y = -1;
            }
        }
        else
        {
            // 현재 아이템 버리기
            itemUi.gameObject.SetActive(false);
            _gameManager.DropInventoryItem(_itemIndexOnSlot[_selectedItemStartIndex.y, _selectedItemStartIndex.x]);
        }
    }
    
    private void ChangePosition()
    {
        if (_selectedItemData == null)
        {
            return;
        }

        int index = _itemIndexOnSlot[_selectedItemStartIndex.y, _selectedItemStartIndex.x];
        _inventoryManager.ChangePosition(index, _hoverInfo.y, _hoverInfo.x);
    }

    private void UpdateInventoryUiState()
    {
        Debug.Log("===update ui state===");
        Debug.Log($"selectedItemStartIdx : {_selectedItemStartIndex}");
        Debug.Log($"_hoverInfo : {_hoverInfo}");
        Debug.Log("===update ui state===");

        int itemIndex = _itemIndexOnSlot[_selectedItemStartIndex.y, _selectedItemStartIndex.x];
        for (int i = _selectedItemStartIndex.y; i < _selectedItemStartIndex.y + _selectedItemData.Height; i++)
        {
            for (int j = _selectedItemStartIndex.x; j < _selectedItemStartIndex.x + _selectedItemData.Width; j++)
            {
                _itemIndexOnSlot[i, j] = -1;
            }
        }

        for (int i = _hoverInfo.y; i < _hoverInfo.y + _selectedItemData.Height; i++)
        {
            for (int j = _hoverInfo.x; j < _hoverInfo.x + _selectedItemData.Width; j++)
            {
                _itemIndexOnSlot[i, j] = itemIndex;
            }
        }

        _selectedItemStartIndex.y = -1;
        _selectedItemStartIndex.x = -1;
        _selectedItemData = null;
    }

    //아이템 생성, 위치 변경시 호출할 함수
    private Vector2 GetItemLocalPosition(int row, int col, int width, int height)
    {
        Vector2 cellSize = _gridLayout.cellSize;
        float xPos = cellSize.x * col + cellSize.x * 0.5f * width;
        float yPos = -cellSize.y * row - cellSize.y * 0.5f * height;
        //position of items is pivot
        return new Vector2(xPos, yPos);
    }

    private void ShowHoveredBlocks()
    {
        if (_hoverInfo.y <= -1 || _hoverInfo.y >= maxRows || _hoverInfo.x >= maxCols || _hoverInfo.x <= -1)
        {
            return;
        }

        Item item = _selectedItemData;
        bool canPosition = CanPosition(
            row: _hoverInfo.y,
            col: _hoverInfo.x,
            width: item.Width,
            height: item.Height
        );
        Color color = canPosition ? canPositionColor : canNotPositionColor;

        for (int i = _hoverInfo.y; i < _hoverInfo.y + item.Height && i < maxRows; i++)
        {
            for (int j = _hoverInfo.x; j < _hoverInfo.x + item.Width && j < maxCols; j++)
            {
                Image img = _slots[i, j].ImageTransform;
                img.color = color;
            }
        }
    }


    private void HideHoveredBlocks()
    {
        if (_hoverInfo.y <= -1 || _hoverInfo.y >= maxRows || _hoverInfo.x >= maxCols || _hoverInfo.x <= -1)
        {
            return;
        }

        Item item = _selectedItemData;
        for (int i = _hoverInfo.y; i < _hoverInfo.y + item.Height && i < maxRows; i++)
        {
            for (int j = _hoverInfo.x; j < _hoverInfo.x + item.Width && j < maxCols; j++)
            {
                Image img = _slots[i, j].ImageTransform;
                img.color = Color.white;
            }
        }
    }

    private bool CanPosition(int row, int col, int width, int height)
    {
        //현재 호버 위치에 1개의 아이템이 존재하면 false
        int currentItemIndex = _itemIndexOnSlot[_selectedItemStartIndex.y, _selectedItemStartIndex.x];
        for (int i = row; i < row + height; i++)
        {
            for (int j = col; j < col + width; j++)
            {
                if (_itemIndexOnSlot[i, j] != -1)
                {
                    if (_itemIndexOnSlot[i,j] != currentItemIndex)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    private Vector2Int GetInventoryPositionAt(Vector2 screenPosition)
    {
        Vector2Int result;
        if (IsInInventoryWindow(screenPosition))
        {
            var position = transform.position;
            int x = (int)((screenPosition.x - position.x) / _gridLayout.cellSize.x);
            int y = -(int)((screenPosition.y - position.y) / _gridLayout.cellSize.y);
            result = new Vector2Int(x, y);
        }
        else
        {
            result = new Vector2Int(-1, -1);
        }


        if (result.x + _selectedItemData.Width >= maxCols)
        {
            result.x -= (result.x + _selectedItemData.Width - maxCols);
        }

        if (result.y + _selectedItemData.Height >= maxRows)
        {
            result.y -= (result.y + _selectedItemData.Height - maxRows);
        }

        return result;
    }

    private bool IsInInventoryWindow(Vector2 screenPosition)
    {
        Vector2 position = transform.position;
        RectTransform rectTransform = GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        float width = rect.width;
        float height = rect.height;
        return screenPosition.x >= position.x &&
               screenPosition.x <= position.x + width &&
               screenPosition.y <= position.y &&
               screenPosition.y >= position.y - height;
    }
}