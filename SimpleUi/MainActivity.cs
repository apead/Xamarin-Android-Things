using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Things.Pio;
using Android.Util;
using Java.IO;

namespace SimpleUi
{
    [Activity(Label = "SimpleUi", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string TAG = "SimpleUi";
        private Dictionary<string, Gpio> _gpioMap = new Dictionary<string, Gpio>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_main);

            var gpioPinsView = (LinearLayout) FindViewById(Resource.Id.gpio_pins);
            var inflater = LayoutInflater;
            var pioService = new PeripheralManagerService();


            foreach (var gpIoName in pioService.GpioList)
            {
                var child = inflater.Inflate(Resource.Layout.list_item_gpio, gpioPinsView, false);
                var button = (Switch) child.FindViewById(Resource.Id.gpio_switch);
                button.Text = gpIoName;
                gpioPinsView.AddView(button);

                Log.Info(TAG, "Added button for GPIO: " + gpIoName);

                try
                {
                    var ledPin = pioService.OpenGpio(gpIoName);
                    ledPin.SetEdgeTriggerType(Gpio.EdgeNone);
                    ledPin.SetActiveType(Gpio.ActiveHigh);
                    ledPin.SetDirection(Gpio.DirectionOutInitiallyLow);

                    button.CheckedChange += (sender, args) =>
                    {
                        try
                        {
                            ledPin.Value = args.IsChecked;

                        }
                        catch (IOException e)
                        {
                            Log.Error(TAG, "error toggling gpio:", e);
                        }
                    };

                    _gpioMap[gpIoName] = ledPin;
                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Error initializing GPIO: " + gpIoName, e);
                    button.Enabled = false;
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var gpio in _gpioMap)
            {
                try
                {
                    gpio.Value.Close();

                }
                catch (IOException e)
                {
                    Log.Error(TAG, "Error closing GPIO " + gpio.Key, e);
                }

                _gpioMap.Clear();
            }
        }
    }
}

