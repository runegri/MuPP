using System;
using System.Globalization;
using System.Threading;

namespace AtB
{
    public class StopTime
    {

        private const string DateFormat = "dd/MM/yyyy HH:mm";

        public string RouteNr { get; private set; }
        public string RouteName { get; private set; }
        public DateTime Time { get; private set; }
        public StopTimeType TimeType { get; private set; }

        public StopTime(string routeNr, string routeName, string time, string timeType)
        {
            RouteNr = routeNr;
            RouteName = routeName;
            Time = ParseRouteTime(time);
            TimeType = timeType.Equals("sched") ? StopTimeType.Schedule : StopTimeType.RealTime;
        }

        private static DateTime ParseRouteTime(string time)
        {
            return DateTime.ParseExact(time, DateFormat, Thread.CurrentThread.CurrentUICulture, DateTimeStyles.AssumeLocal);
        }

        public override string ToString()
        {
            var result = "Rute " + RouteNr + " kl " + Time.ToShortTimeString() + (TimeType == StopTimeType.Schedule ? "" : "*");

            var timeDiff = Time - DateTime.Now;
            if (timeDiff.TotalMinutes < 30 && timeDiff.TotalMinutes > 0)
            {
                result += " (" + (int)timeDiff.TotalMinutes + " min)";
            }

            return result;
        }

        public string TimeString
        {
            get
            {

                var timeDiff = Time - DateTime.Now;
                var timeDiffString = "";
                if (timeDiff.TotalMinutes < 30 && timeDiff.TotalMinutes > 0)
                {
                    timeDiffString = " (" + (int)timeDiff.TotalMinutes + " min)";
                }
                return Time.ToShortTimeString() + timeDiffString;
            }
        }

        public string TimeTypeString
        {
            get { return TimeType == StopTimeType.RealTime ? "(Sanntid)" : "(Rutetid)"; }
        }

        public string RouteNrString
        {
            get { return "Rute " + RouteNr; }
        }
    }

    public enum StopTimeType
    {
        RealTime,
        Schedule
    }
}

