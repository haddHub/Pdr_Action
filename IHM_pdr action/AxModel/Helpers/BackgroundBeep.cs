using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AxModel.Helpers
{
    public class BackgroundBeep : IDisposable
    {
        static Thread _beepThread;
        static AutoResetEvent _timerBeep;
        static int _frequency = 1000;
        static int _duration = 1000;

        static BackgroundBeep()
        {
            _timerBeep = new AutoResetEvent(false);
            _beepThread = new Thread(() =>
            {
                for (; ; )
                {
                    _timerBeep.WaitOne();
                    Console.Beep(_frequency, _duration);
                }
            }, 1);
            _beepThread.IsBackground = true;
            _beepThread.Name = "Beep Thread";
            _beepThread.Start();
        }

        //public static void setBeep(int frequency, int duration)
        //{
        //    _frequency = frequency;
        //    _duration = duration;
        //}

        public static void Beep(int frequency, int duration)
        {
            _frequency = frequency;
            _duration = duration;
            _timerBeep.Set();
        }

        public void Dispose()
        {
            if (_beepThread.IsAlive)
            {
                _beepThread.Abort();
            }
        }
    }
}
