using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using SQLite;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace AtB
{
    public interface ISanntidData
    {
        void GetBusStopList(Action<List<BusStop>> callback);
        void GetRealTimeData(BusStop busStop, Action<List<StopTime>> callback);
    }

    public class Sanntid : ISanntidData
    {

        private const string UserName = "user";
        private const string Password = "pass";
        private const string UserId = "runegri";
		private const string DbName = "Sanntid.db";
		
		private readonly SQLiteConnection _conn;
		
		public Sanntid()
		{
			try
			{
				var dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), DbName);
				_conn = new SQLiteConnection(dbPath);
				_conn.EnsureTableExists<BusStop>();
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}
		
        private HandShake _handShake;
        private HandShake GetHandShake()
        {
            if (_handShake == null)
            {
				_handShake = new UserServices().getToken(Authentication, UserId);
            }
            return _handShake;
		}

        private static WsAuthentication Authentication
        {
            get { return new WsAuthentication { user = UserName, password = Password }; }
        }
        
        public void GetBusStopList(Action<List<BusStop>> callback)
        {
			var stopsInDb = GetBusStopsFromDb();
			if (stopsInDb.Any())
			{
				callback(stopsInDb);
			}
			else
			{
	            var client = new UserServices();
				client.GetBusStopsListCompleted += (s,e) =>
	            {
	                var json = e.Result; 
					var busStopsFromJson = new BusStopListConverter().GetBusStopsList(json);
					AddStopsToDb(busStopsFromJson);
					callback(busStopsFromJson);
	            };
				client.GetBusStopsListAsync(Authentication);
			}
		}
		
		private List<BusStop> GetBusStopsFromDb()
		{
			try
			{
				return _conn.Table<BusStop>().ToList();			
			}
			catch(Exception ex)
			{
				throw;
			}
		}
		
		private void AddStopsToDb(IEnumerable<BusStop> busStops)
		{
			ThreadPool.QueueUserWorkItem(o => _conn.InsertAll(busStops));
		}
		
        public void GetRealTimeData(BusStop busStop, Action<List<StopTime>> callback)
        {

            var client = new UserServices();
            client.getUserRealTimeForecastCompleted += (s, e) =>
            {
                var json = e.Result; 
                var stopTimes = new StopTimeConverter().GetStopTimes(json);
                callback(stopTimes);
            };
            client.getUserRealTimeForecastAsync(Authentication, GetHandShake(), busStop.Id);
        }


    }
}
