using System.Collections.Generic;
using Model.Item;

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

        public void RemoveItem(InventoryItem item)
        {
            Items.Remove(item);
            
        }
    }
}