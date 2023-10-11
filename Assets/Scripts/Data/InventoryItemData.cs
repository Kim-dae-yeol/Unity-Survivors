using Model.Item;

namespace Data
{
    public class InventoryItemData
    {
        public int Row;
        public int Col;

        public Item.ItemType Type;
        public int Width;
        public int Height;

        public string ItemName;
        public string ItemDesc;
        public int MinDamage;
        public int MaxDamage;

        public InventoryItem ToDTO()
        {
            Item item = new Item(Type, Width, Height, ItemName, ItemDesc, MinDamage, MaxDamage);
            return new InventoryItem(row: Row, col: Col, item: item);
        }
    }
}