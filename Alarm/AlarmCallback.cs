using System.Threading;
using Android.Things.Pio;
using Android.Util;

namespace Alarm
{
    public class AlarmCallback : GpioCallback
    {
        public Gpio BuzzerPin { get; set; }

        public override  bool OnGpioEdge(Gpio p0)
        {
            if (p0.Value)
            {
                if (!BuzzerPin.Value)
                {
                        BuzzerPin.Value = true;
                        Thread.Sleep(1000);
                        BuzzerPin.Value = false;
                }
            }

            return true;
        }

        public override void OnGpioError(Gpio p0, int p1)
        {
            Log.Info("Alarmcallback", "Error");
        }
    }
}