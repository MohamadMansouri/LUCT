using LUCT.DatabaseModels;
using LUCT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.ViewModels
{
    public class Data
    {
        public string LineCategory { get; set; }

        public DateTime BarCategory { get; set; }

        public double LineValue { get; set; }

        public double BarValue { get; set; }
    }

    public class ChartViewModel
    {
        private AppDbContext _database;
        public DataByEnum DataBy { get; set; }
        public DataByType DataType { get; set; }
        public ChartViewModel()
        {
            _database = new AppDbContext();
        }
        public ObservableCollection<Data> GetData
        {
            get
            {
                return GetDataBy(DataBy);
            }
        }
        public ObservableCollection<Data> GetDataBy(DataByEnum db)
        {
            List<SensorsData> sensorsData = _database.GetModel<SensorsData>();
            switch (db)
            {
                case DataByEnum.Day:
                    sensorsData = (from d in sensorsData
                                  where (d.Time - DateTime.Now).TotalDays < 1
                                  select d).ToList();
                    var c1 = sensorsData.Count;
                    sensorsData = GetAverage(sensorsData, c1/6);
                    break;
                case DataByEnum.Week:
                    sensorsData = (from d in sensorsData
                                   where (d.Time - DateTime.Now).TotalDays < 7
                                   select d).ToList();
                    var c2 = sensorsData.Count;
                    sensorsData = GetAverage(sensorsData, c2/6);
                    break;
                case DataByEnum.Month:
                    sensorsData = (from d in sensorsData
                                   where (d.Time - DateTime.Now).TotalDays < 360
                                   select d).ToList();
                    var c3 = sensorsData.Count;
                    sensorsData = GetAverage(sensorsData, c3/6);
                    break;
            }
            ObservableCollection<Data> collection = new ObservableCollection<Data>();
            foreach (var s in sensorsData)
            {
                switch (DataType)
                {
                    case DataByType.Humidity:
                        collection.Add(new Data { BarCategory = s.Time, BarValue = s.Humidity });
                        break;
                    case DataByType.Temperature:
                        collection.Add(new Data { BarCategory = s.Time, BarValue = s.Temperature });
                        break;
                    case DataByType.TankLevel:
                        collection.Add(new Data { BarCategory = s.Time, BarValue = s.TankLevel });
                        break;
                }
            }
                return collection;
        }

        public List<SensorsData> GetAverage(List<SensorsData> list, int nElement)
        {
            var currentElement = 0;
            var tempSum = 0.0;
            var humSum = 0.0;
            var tankSum = 0.0;

            var newList = new List<SensorsData>();

            foreach (var item in list)
            {
                tempSum += item.Temperature;
                humSum += item.Humidity;
                tankSum += item.TankLevel;
                currentElement++;

                if (currentElement == nElement)
                {
                    newList.Add(new SensorsData() {Time = item.Time, Temperature = tempSum / nElement,Humidity = humSum/nElement, TankLevel = tankSum/nElement });
                    currentElement = 0;
                    tempSum = 0.0;
                    humSum = 0.0;
                    tankSum = 0.0;
                }
            }
            return newList;
        }
    }
    public enum DataByEnum
    {
        Day,
        Week,
        Month
    }
    public enum DataByType
    {
        Temperature,
        Humidity,
        TankLevel
    }
}
