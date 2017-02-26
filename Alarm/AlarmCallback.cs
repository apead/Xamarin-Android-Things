using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Things.Pio;

namespace Alarm
{
    public class AlarmCallback : Com.Google.Android.Things.Pio.GpioCallback
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