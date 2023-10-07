using System.Collections.Generic;
using DE.Util;
using Model.LevelDesign;

namespace Managers
{
    public class DataManager
    {
        
        public List<TestData> LoadTestDataSet()
        {
            return CsvReader.ReadCsv<TestData>("test.csv", 1);
        }
    }
}