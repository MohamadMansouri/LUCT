using LUCT.FacialRecognition;
using LUCT.Helpers;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Windows.Devices.Gpio;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace LUCT.Services
{
    public class GPIOService
    {
        int led, pushButton;
        GpioPin Ledpin, PushButtonpin;
        List<string> recognizedVisitors = new List<string>();
        AppDbContext database = new AppDbContext();
        GpioPinValue oldvalue;
        WebcamHelper webcam;
        bool isCapturing = false;
        int i = 0;
        public GPIOService()
        {
            led = 17;
            pushButton = 22;
            InitGpio();

            webcam = new WebcamHelper();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (!isCapturing)
            {
                if (PushButtonpin != null)
                {
                    var val = PushButtonpin.Read();
                    if (i > 20 && val == GpioPinValue.Low && oldvalue == GpioPinValue.High)
                    {
                        isCapturing = true;
                        await CapturePhoto();
                    }
                    if (val == GpioPinValue.High && oldvalue == GpioPinValue.High)
                        i++;
                    else
                        i = 0;
                    oldvalue = val;
                }
            }
            
        }

        public void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                PushButtonpin = null;
                Ledpin = null;
                return;
            }
            Ledpin = gpio.OpenPin(led);
            PushButtonpin = gpio.OpenPin(pushButton);

            Ledpin.Write(GpioPinValue.High);
            Ledpin.SetDriveMode(GpioPinDriveMode.Output);
            Ledpin.Write(GpioPinValue.High);


            //if (PushButtonpin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
            //    PushButtonpin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            //else
            PushButtonpin.SetDriveMode(GpioPinDriveMode.Input);
            //PushButtonpin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            //PushButtonpin.ValueChanged += buttonPin_ValueChanged;

            
        }

        private async void buttonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                
            }
        }

        public async Task InitializeOxf()
        {
            //await OxfordFaceAPIHelper.InitializeOxford();
        }

        public async Task CapturePhoto()
        {
            await webcam.InitializeCameraAsync();
            if (!webcam.IsInitialized())
                Debug.WriteLine("Not initialied");
                StorageFile image = await webcam.CapturePhoto();
            try
            {
                // Oxford determines whether or not the visitor is on the Whitelist and returns true if so
                recognizedVisitors = await OxfordFaceAPIHelper.IsFaceInWhitelist(image);
            }
            catch (FaceRecognitionException fe)
            {
                switch (fe.ExceptionType)
                {
                    // Fails and catches as a FaceRecognitionException if no face is detected in the image
                    case FaceRecognitionExceptionType.NoFaceDetected:
                        Debug.WriteLine("WARNING: No face detected in this image.");
                        break;
                }
            }
            catch (FaceAPIException faceAPIEx)
            {
                Debug.WriteLine("FaceAPIException in IsFaceInWhitelist(): " + faceAPIEx.ErrorMessage);
            }
            catch
            {
                // General error. This can happen if there are no visitors authorized in the whitelist
                Debug.WriteLine("WARNING: Oxford just threw a general expception.");
            }

            if (recognizedVisitors.Count > 0)
            {
                // If everything went well and a visitor was recognized, unlock the door:
                Ledpin.Write(GpioPinValue.Low);
            }
            else
            {
                // Create or open the folder in which the Whitelist is stored
                StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Banned", CreationCollisionOption.OpenIfExists);
                // Move the already captured photo the user's folder
                await image.MoveAsync(folder);
                var model = new Models.Database.Ban()
                {
                    Image = image.Name,
                    Time = DateTime.Now

                };
                Ledpin.Write(GpioPinValue.High);
                database.InsertModel<Models.Database.Ban>(model);
            }
            isCapturing = false;
        }

        public void ChangeLedState(bool on)
        {
            if (on)
                Ledpin.Write(GpioPinValue.Low);
            else
                Ledpin.Write(GpioPinValue.High);
        }

    }
}
