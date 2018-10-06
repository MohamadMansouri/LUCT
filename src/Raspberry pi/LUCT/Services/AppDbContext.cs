using LUCT.DatabaseModels;
using LUCT.Models;
using LUCT.Models.Database;
using LUCT.ViewModels;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LUCT.Services
{
    public class AppDbContext
    {
        private string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.sqlite");
        private SQLiteConnection database;
        public AppDbContext()
        {
            if (!File.Exists(path))
            {
                CreateDatabase();
            }
            else
            {
                database = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            }
        }
        private void CreateDatabase()
        {
            database = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            database.CreateTable<Security>();
            database.CreateTable<TankLevel>();
            database.CreateTable<SensorsData>();
            database.CreateTable<Ban>();
            database.CreateTable<Sms>();
            database.CreateTable<Pills>();
            database.CreateTable<Rfid>();
            database.CreateTable<Events>();
            database.CreateTable<PillsTaken>();
            AddInitialData();
        }
        private void AddInitialData()
        {
            database.Insert(new Security() { KeyPadPassword = "12345",Password = "1234",BanQuota=3,BanTime=20 });
            database.Insert(new TankLevel() { Height = 10 });

            //Ban sample data

            database.Insert(new Models.Database.Pills() { Id = 1, Name = "Pill1", Time = DateTime.Now });
            database.Insert(new Models.Database.Pills() { Id = 1, Name = "Pill1", Time = DateTime.Now.AddHours(2) });
            database.Insert(new Models.Database.Pills() { Id = 1, Name = "Pill1", Time = DateTime.Now.AddDays(3) });

            database.Insert(new SensorsData() { Current = 20, Humidity = 20, Temperature = 20, TankLevel = 5, Time = DateTime.Now });
            //Sms
            database.Insert(new Sms());
        }
        public bool SetPassword(string password)
        {
            database.Update(new Security()
            {
                Password = password
            });
        return true;
        }
        public bool CheckPassword(string password)
        {
            var realPassword = database.Table<Security>().Where(i => i.Id == 0).Select(i => i.Password).ToList()[0];
            if (password == realPassword)
                return true;
            return false;
        }
        //public double GetTankHeight()
        //{
        //    return database.Table<TankLevel>().FirstOrDefault().Height;
        //}
        
        public void SetSensorValues(SensorsViewModel model)
        {
          database.Insert(new SensorsData()
                {
                    Temperature = model.Temperature,
                    Humidity = model.Humidity,
                    TankLevel = model.TankLevel,
                    Time = model.Time
         });
        }
        //public List<Ban> GetBanData()
        //{
        //    return database.Table<Ban>().ToList();
        //}
        //public List<SensorsData> GetSensorValues()
        //{
        //    return database.Table<SensorsData>().ToList();
        //}
        public void UpdatePassword(string password)
        {
            database.Update(new Security() { Password = password});
        }


        //public Sms GetSmsData()
        //{
        //    return database.Table<Sms>().ToList()[0];
        //}

        public T GetFirstModel<T>() where T:class
        {
            return database.Table<T>().FirstOrDefault();
        }
        public List<T> GetModel<T>() where T:class
        {
            return database.Table<T>().ToList();
        }
        public T GetModelLast<T>() where T : class
        {
            if(database.Table<T>().Any())
                return database.Table<T>().First();
            return null;
        }
        public void UpdateModel<T>(T model)
        {
            database.Update(model);
        }
        public void InsertModel<T>(T model)
        {
            database.Insert(model);
        }

        public void DeleteModel<T>(int key) where T:class
        {
            database.Delete<T>(key);
        }
        public void DeleteLastRfid()
        {
            var item = database.Table<Rfid>().ToList().Last();
            database.Delete(item.Id); 
        }
    }
}
