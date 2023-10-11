using System.Collections.Generic;
using System.Linq;
using Data;
using DE.Util;
using Model.Item;

namespace Managers
{
    public class DataManager
    {
        public List<InventoryItem> LoadTestDataSet()
        {
            List<InventoryItemData> items = CsvReader.ReadCsv<InventoryItemData>("test.csv", 1);
            return items.Select(data => data.ToDTO()).ToList();
        }
    }
}