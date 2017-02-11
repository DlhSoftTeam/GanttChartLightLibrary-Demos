using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Demos
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            startTime = DateTime.Now;

            var time = startTime.Date;
            while (time.DayOfWeek != DayOfWeek.Monday)
                time = time.AddDays(-1);
            time = time.AddHours(12);
            var systemTime = new SYSTEMTIME { wYear = (ushort)time.Year, wMonth = (ushort)time.Month, wDay = (ushort)time.Day, wHour = (ushort)time.Hour };
            SetLocalTime(ref systemTime);

            updatedStartTime = DateTime.Now;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var time = startTime.Add(DateTime.Now - updatedStartTime);

            var systemTime = new SYSTEMTIME { wYear = (ushort)time.Year, wMonth = (ushort)time.Month, wDay = (ushort)time.Day, wHour = (ushort)time.Hour, wMinute = (ushort)time.Minute, wSecond = (ushort)time.Second, wMilliseconds = (ushort)time.Millisecond };
            SetLocalTime(ref systemTime);

            base.OnExit(e);
        }

        private DateTime startTime;
        private DateTime updatedStartTime;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetLocalTime(ref SYSTEMTIME lpSystemTime);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }
    }
}
