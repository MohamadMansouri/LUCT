using LUCT.DatabaseModels;
using LUCT.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace LUCT.Services
{
    public class I2cHelper
    {
        private static bool Lock = false;
        private static string AQS;
        private static DeviceInformationCollection DIS;
        private static AppDbContext _database = new AppDbContext();
        /// <summary>
        /// Defines protocol mode for the communication.
        /// </summary>
        public enum Mode : byte
        {
            /// <summary>
            /// Retrieves sensor data from specified I2C slave Arduino
            /// </summary>
            Mode0 = 0,
            /// <summary>
            /// Retrieves devices state from specified I2C slave Arduino
            /// </summary>
            Mode1 = 1,
            /// <summary>
            /// Sends IO signal to pin of specified I2C slave Arduino
            /// </summary>
            Mode2 = 2
        }

        /// <summary>
        /// Sends control signal to the specific Arduino and retrieves response bytes.
        /// </summary>
        /// <param name="slave">Slave to which data to be send</param>
        /// <param name="ControlMode">Select specific control mode.</param>
        /// <param name="Pin">Pin to be set. ONLY VALID FOR MODE2</param>
        /// <param name="PinValue">Value to be set. ONLY VALID FOR MODE2</param>
        /// <returns>Returns fourteen response byte.</returns>
        public static async System.Threading.Tasks.Task WriteRead(Slave slave, Mode ControlMode, byte Pin = 0, byte PinValue = 0,SmsSlaveModel model = null)
        {
            while (Lock != false)
            {

            }


            Lock = true;
            // Create response byte array of fourteen
            byte[] Response2 = new byte[8];
            byte[] Response1 = new byte[3];

            try
            {
                // Initialize I2C
                var Settings = new I2cConnectionSettings(slave.I2cAddress);
                Settings.BusSpeed = I2cBusSpeed.FastMode;

                if (AQS == null || DIS == null)
                {
                    AQS = I2cDevice.GetDeviceSelector();
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }

                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    byte[] writeValues = PrepareWriteBytes(slave.I2cAddress,model);
                    if(slave.I2cAddress == 8)
                    {
                        //foreach(var w in writeValues)
                            Device.Write(writeValues);
                        Device.Read(Response1);
                        DecodeRespose(slave.I2cAddress, Response1);
                    }
                    if (slave.I2cAddress == 9)
                    {
                        Device.Write(writeValues);
                        Device.Read(Response2);
                        DecodeRespose(slave.I2cAddress, Response2);
                    }
                    if (slave.I2cAddress == 8)
                    {
                        AppKeys.Slave1Reachable = new SolidColorBrush(Colors.Green);
                        AppKeys.Slave1State = "Connected";
                    }
                    if (slave.I2cAddress == 9)
                    {
                        AppKeys.Slave2Reachable = new SolidColorBrush(Colors.Green);
                        AppKeys.Slave2State = "Connected";
                    }
                }
            }
            catch (Exception ex)
            {
                if(slave.I2cAddress == 8)
                {
                    AppKeys.Slave1Reachable = new SolidColorBrush(Colors.Red);
                    AppKeys.Slave1State = "Checking ...";
                }
                if (slave.I2cAddress == 9)
                {
                    AppKeys.Slave2Reachable = new SolidColorBrush(Colors.Red);
                    AppKeys.Slave2State = "Checking ...";
                }

                // SUPPRESS ERROR AND RETURN EMPTY RESPONSE ARRAY
            }

            Lock = false;
            //return Response;
        }

        public static async System.Threading.Tasks.Task Read(Slave slave)
        {
            while (Lock != false)
            {

            }


            Lock = true;
            // Create response byte array of fourteen
            byte[] Response2 = new byte[8];
            byte[] Response1 = new byte[3];

            try
            {
                // Initialize I2C
                var Settings = new I2cConnectionSettings(slave.I2cAddress);
                Settings.BusSpeed = I2cBusSpeed.FastMode;

                if (AQS == null || DIS == null)
                {
                    AQS = I2cDevice.GetDeviceSelector();
                    DIS = await DeviceInformation.FindAllAsync(AQS);
                }

                using (I2cDevice Device = await I2cDevice.FromIdAsync(DIS[0].Id, Settings))
                {
                    byte[] writeValues = PrepareWriteBytes(slave.I2cAddress);
                    if (slave.I2cAddress == 8)
                    {
                        //foreach(var w in writeValues)
                        Device.Write(writeValues);
                        Device.Read(Response1);
                        DecodeRespose(slave.I2cAddress, Response1);
                    }
                    if (slave.I2cAddress == 9)
                    {
                        Device.Read(Response2);
                        DecodeRespose(slave.I2cAddress, Response2);
                    }
                    if (slave.I2cAddress == 8)
                        AppKeys.Slave1Reachable = new SolidColorBrush(Colors.Green);
                    if (slave.I2cAddress == 9)
                        AppKeys.Slave2Reachable = new SolidColorBrush(Colors.Green);
                }
            }
            catch (Exception ex)
            {
                if (slave.I2cAddress == 8)
                    AppKeys.Slave1Reachable = new SolidColorBrush(Colors.Red);
                if (slave.I2cAddress == 9)
                    AppKeys.Slave2Reachable = new SolidColorBrush(Colors.Red);

                // SUPPRESS ERROR AND RETURN EMPTY RESPONSE ARRAY
            }

            Lock = false;
            //return Response;
        }
        public static async Task<IEnumerable<byte>> FindDevicesAsync()
        {
            IList<byte> returnValue = new List<byte>();
            // *** 
            // *** Get a selector string that will return all I2C controllers on the system 
            // *** 
            string aqs = I2cDevice.GetDeviceSelector();
            // *** 
            // *** Find the I2C bus controller device with our selector string 
            // *** 
            var dis = await DeviceInformation.FindAllAsync(aqs).AsTask();
            if (dis.Count > 0)
            {
                const int minimumAddress = 8;
                const int maximumAddress = 77;
                for (byte address = minimumAddress; address <= maximumAddress; address++)
                {
                    var settings = new I2cConnectionSettings(address);
                    settings.BusSpeed = I2cBusSpeed.FastMode;
                    settings.SharingMode = I2cSharingMode.Shared;
                    // *** 
                    // *** Create an I2cDevice with our selected bus controller and I2C settings 
                    // *** 
                    using (I2cDevice device = await I2cDevice.FromIdAsync(dis[0].Id, settings))
                    {
                        if (device != null)
                        {
                            try
                            {
                                byte[] writeBuffer = new byte[1] { 1 };
                                device.Write(writeBuffer);
                                // *** 
                                // *** If no exception is thrown, there is 
                                // *** a devie at this address. 
                                // *** 
                                returnValue.Add(address);
                            }
                            catch
                            {
                                // *** 
                                // *** If the address is invalid, an exception will be thrown. 
                                // *** 
                            }
                        }
                    }
                }
            }
            return returnValue;
        }
        public static async void DecodeRespose(int slaveId, byte[] response)
        {
            if (slaveId == 8)
            {
                if (response[0] == 1)
                {
                    await App.LUCTGPIOService.CapturePhoto();
                    await I2cHelper.WriteRead(new Slave() { I2cAddress = 9 }, Mode.Mode1, 0, 0, new SmsSlaveModel() { Sflag = Sflag.BadKeyPadAttempts, SmsSend = 1, Switch1 = -1, Switch2 = -1 });
                }
                if (response[1] == 1 || response[2] == 1)
                    App.LUCTGPIOService.ChangeLedState(true);
            }
            else
            {
                SensorsData sensor = new SensorsData()
                {
                    TankLevel = response[0],
                    Temperature = response[1],
                    Humidity = response[2],
                    Current = (((response[4]+response[5]))*220*0.9)/1000
                };
                Debug.WriteLine(response[0]);
                SensorsBinder.dt = sensor;
                _database.InsertModel<SensorsData>(sensor);
                if ((response[4]) * 220 * 0.9 > 4000)
                {
                    AppKeys.Switch1State = true;
                    //AppKeys.Switch1 = true ;
                }
                else
                {
                    AppKeys.Switch1State = false;
                    //AppKeys.Switch1 = false;
                }
                if ((response[5]) * 220 * 0.9 > 4000)
                {
                    AppKeys.Switch2State = true;
                    //AppKeys.Switch2 = true;
                }
                else
                {
                    AppKeys.Switch2State = false;
                    //AppKeys.Switch2 = false;
                }
                if (response[7] == 1)
                {
                    _database.InsertModel<Models.Database.PillsTaken>(new Models.Database.PillsTaken() { Time = DateTime.Now });
                    AppKeys.PillState = true;
                }
                else
                {
                    AppKeys.PillState = false;
                }

                HandleRFlag(response[6]);

                Debug.WriteLine(response[4]* 220 * 0.9 + " From i2c switch 1");
                Debug.WriteLine(response[5]* 220 * 0.9 + "From i2c switch 2");

            }
        }

        public static async void HandleRFlag(byte r)
        {
            if (r == 0)
                await I2cHelper.WriteRead(new Slave() { I2cAddress = 9 },Mode.Mode1,0,0, new SmsSlaveModel() { Sflag = 0, SmsSend = 1, Switch1 = AppKeys.Switch1 ? 1:0, Switch2 = AppKeys.Switch2 ? 1 : 0 });
            else if (r ==1)
            {
                int s1 =1, s2=1;
                if (AppKeys.Switch1 == true)
                    s1 = 0;
                if (AppKeys.Switch2 == true)
                    s2 = 0;
                AppKeys.Switch2 = AppKeys.Switch1 = false;
                await I2cHelper.WriteRead(new Slave() { I2cAddress = 9 }, Mode.Mode1, 0, 0, new SmsSlaveModel() { Sflag = Sflag.Shutdowned, SmsSend = 1, Switch1 = s1, Switch2 = s2 });
            }
            else if(r == 2)
            {
                int s1 = 0,s2=0;
                if (AppKeys.Switch1 == false)
                    s1 = 1;
                if (AppKeys.Switch2 == true)
                    s2 = 1;
                AppKeys.Switch1 = false;
                await I2cHelper.WriteRead(new Slave() { I2cAddress = 9 }, Mode.Mode1, 0, 0, new SmsSlaveModel() { Sflag = Sflag.SleepMode, SmsSend = 1, Switch1 = s1, Switch2 = s2 });
            }
        }
        public static byte[] PrepareWriteBytes(int salveId,SmsSlaveModel model = null)
        {
            List<byte> byteArray = new List<byte>();
            if (salveId == 8)
            {
                var security = _database.GetModel<Security>()[0];
                var rfids = _database.GetModel<Models.Database.Rfid>();

                byteArray.Add((byte)security.BanQuota);
                byteArray.Add((byte)security.BanTime);
                byteArray.Add((byte)security.KeyPadPassword.Length);
                foreach(var c in security.KeyPadPassword)
                {
                    byteArray.Add((byte)(c));
                }
                //byteArray.Add((byte)rfids.Count);
                //foreach (var r in rfids)
                //{
                //    foreach(var rf in r.Text)
                //    {
                //        byteArray.Add((byte)Int32.Parse(rf.ToString()));
                //    }
                //}
            }
            else
            {
                byteArray.Add((byte)model.Sflag);
                byteArray.Add((byte)model.Switch1);
                byteArray.Add((byte)model.Switch2);
                byteArray.Add((byte)model.SmsSend);
                //byteArray.Add((byte)11);
                //var nb = "96171193064";
                //foreach(var n in nb)
                //    byteArray.Add((byte)n);
            }
            return byteArray.ToArray();
        }
        public enum Sflag
        {
            None,
            Shutdowned,
            SleepMode,
            PillTime,
            BadKeyPadAttempts,
            EmptyTank
        }
        public class SmsSlaveModel
        {
            public Sflag Sflag { get; set; }
            public int Switch1 { get; set; }
            public int Switch2 { get; set; }
            public int SmsSend { get; set; }
        }
    }
}
