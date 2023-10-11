namespace Model.Item
{
    public class Item
    {
        public enum ItemType
        {
            Weapon,
            Helm,
            Armor,
            Ring,
            SubWeapon,
            Neck,
            Belt,
            Shoes,
        }
        
        public ItemType Type;
        public int Width;
        public int Height;

        public string ItemName;
        public string ItemDesc;
        public int MinDamage;
        public int MaxDamage;
        
        //todo special effect 
        public Item(ItemType type, int width, int height, string itemName, string itemDesc, int minDamage, int maxDamage)
        {
            Type = type;
            Width = width;
            Height = height;
            ItemName = itemName;
            ItemDesc = itemDesc;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }
    }
}