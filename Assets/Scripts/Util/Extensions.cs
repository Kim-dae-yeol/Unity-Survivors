using Model.Item;
using Model.UI;
using UI.InteractableUi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Util
{
    public static class Extensions
    {
        public static Vector2 GetPosition(this GridLayoutGroup layoutGroup, int row, int col)
        {
            Transform transform = layoutGroup.transform;
            Vector2 position = transform.position;
            Vector2 cellSize = layoutGroup.cellSize;

            position.x += col * cellSize.x;
            position.y += row * cellSize.y;
            return position;
        }

        public static bool IsPositionInItem(this InventoryItem item, int row, int col)
        {
            return item.Row <= row &&
                   item.Row + item.Item.Height >= row &&
                   item.Col <= col &&
                   item.Col + item.Item.Height >= col;
        }
    }
}