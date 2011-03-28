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
            var result = "Rute " + RouteNr + " ";
			result += GetTime();
			result += IsRealTime() ? "*" : "";
			result += GetTimeDifference();

            return result;
        }
		
		public string GetTime()
		{
			return "kl " + Time.ToShortTimeString();	
		}
		
		public string GetTimeDifference()
		{
			string timeDifference = "";
			
            var timeDiff = Time - DateTime.Now;
            if (timeDiff.TotalMinutes < 30 && timeDiff.TotalMinutes > 0)
            {
                timeDifference += " (" + (int)timeDiff.TotalMinutes + " min)";
            }
			
			return timeDifference;
		}
		
		public bool IsRealTime()
		{
			return TimeType == StopTimeType.RealTime; 
		}	
    }

    public enum StopTimeType
    {
        RealTime,
        Schedule
    }
}

