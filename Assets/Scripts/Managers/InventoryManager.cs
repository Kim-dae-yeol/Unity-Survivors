using System.Collections.Generic;
using System.Text;
using Model.Item;
using UnityEngine;
using Util;

namespace Managers
{
    public class InventoryManager
    {
        //state : items
        //event : removeItem, sellItem, addItem
        //todo refactor modeling this class 
        public List<InventoryItem> Items { get; private set; }

        public InventoryManager(List<InventoryItem> items)
        {
            Items = items;
        }

        public void RemoveItemAt(int index)
        {
            Items.RemoveAt(index);
        }

        public void ChangePosition(int index, int row, int col)
        {
            InventoryItem newItem = Items[index];
            newItem.Row = row;
            newItem.Col = col;
            Items[index] = newItem;
            
            Debug.Log("=====inventoryItem=====");
            foreach (var inventoryItem in Items)
            {
                StringBuilder sb = new StringBuilder("Item : ");
                sb.Append("(");
                sb.Append($"{inventoryItem.Row}, ");
                sb.Append($"{inventoryItem.Col}");
                sb.Append(")");
                Debug.Log(sb.ToString());
            }

            Debug.Log("=====inventoryItem=====");
            //todo
        }
    }
}