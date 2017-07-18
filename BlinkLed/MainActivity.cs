using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Things.Pio;
using Android.Util;
using Java.Lang;


namespace BlinkLed
{
    [Activity(Label = "BlinkLed", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string TAG = "MainActivity";
        private Gpio _ledGpio;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Log.Info(TAG, "Starting BlinkActivity");

            var service = new PeripheralManagerService();

            try
            {

                string pinName = BoardDefaults.GetGpioForLed();
                _ledGpio = service.OpenGpio(pinName);
                _ledGpio.SetDirection(Gpio.DirectionOutInitiallyLow);

                Log.Info(TAG, "Start blinking LED GPIO pin");

                Task.Run(() =>
                {
                    while (true)
                    {
                        _ledGpio.Value = !_ledGpio.Value;

                        Log.Info(TAG, "State set to " + _ledGpio.Value);

                        Thread.Sleep(100);
                    }
                });
            }
            catch (Exception e)
            {
                Log.Error(TAG, "Error on PeripheralIO API", e);
            }
        }
    }
}

