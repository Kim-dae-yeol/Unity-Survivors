
namespace Model.Item
{
    public struct InventoryItem
    {
        public int Row;
        public int Col;
        public Item Item;

        public InventoryItem(Item item, int col, int row)
        {
            Item = item;
            Col = col;
            Row = row;
        }
    }
}