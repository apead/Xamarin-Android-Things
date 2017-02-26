using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Com.Google.Android.Things.Pio;

namespace Alarm
{
    [Activity(Label = "Alarm", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private const string TAG = "MainActivity";
        private Gpio _alarm1Gpio;
        private Gpio _buzzerGpio;
        private AlarmCallback _callback;

        public void ConfigureAlarmInput()
        {
            _alarm1Gpio.SetDirection(Gpio.DirectionIn);
            _alarm1Gpio.SetActiveType(Gpio.ActiveHigh);
            _alarm1Gpio.SetEdgeTriggerType(Gpio.EdgeRising);


            _callback = new AlarmCallback {BuzzerPin = _buzzerGpio};

            _alarm1Gpio.RegisterGpioCallback(_callback);
        }
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Log.Info(TAG, "Starting AlarmActivity");

            var service = new PeripheralManagerService();
            _alarm1Gpio = service.OpenGpio("IO8");
            _buzzerGpio = service.OpenGpio("IO4");
            _buzzerGpio.SetDirection(Gpio.DirectionOutInitiallyLow);

            ConfigureAlarmInput();
        }
    }
}

