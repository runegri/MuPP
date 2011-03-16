using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using SQLite;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.IO;
using Wp7Sanntid.AtB;

namespace AtB
{
	
    public class BusStopRepository : IBusStopRepository
    {

        private const string UserName = "user";
        private const string Password = "pass";
        private const string UserId = "runegri";
		private const string DbName = "Sanntid.db";
		private const int NumNearbyStops = 5;
		
		private readonly SQLiteConnection _conn;
		
		public BusStopRepository()
		{
			try
			{
				var dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), DbName);
			    //var dbPath = DbName;
				_conn = new SQLiteConnection(dbPath);
				_conn.EnsureTableExists<BusStop>();
			    ThreadPool.QueueUserWorkItem(o => GetHandShake());
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}
		
        private  object _handShakeLock = new object();
        private HandShake _handShake;        
        private HandShake GetHandShake()
        {
            lock (_handShakeLock)
            {
                if (_handShake == null)
                {
                    UserServicesSoapClient client = GetClient();
                    client.getTokenCompleted += (s, e) =>
                    {
                        lock (_handShakeLock)
                        {
                            _handShake = e.Result;
                            Monitor.Pulse(_handShakeLock);
                        }
                    };
                    client.getTokenAsync(Authentication, UserId);
					Monitor.Wait(_handShakeLock);
                }
            }
            return _handShake;
		}

        private UserServicesSoapClient GetClient()
        {
            return new UserServicesSoapClient(
                new BasicHttpBinding { MaxReceivedMessageSize = 2000000 },
                new EndpointAddress("http://195.0.188.74/InfoTransit/userservices.asmx"));
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
			    var client = GetClient();
				client.GetBusStopsListCompleted += (s,e) =>
	            {
					var busStopsFromJson = new BusStopListConverter().GetBusStopsList(e.Result);
					AddStopsToDb(busStopsFromJson);
					callback(busStopsFromJson);
	            };
				client.GetBusStopsListAsync(Authentication);
			}
		}

        public void GetNearbyStops(GeographicCoordinate location, Action<List<BusStop>> callback)
        {
            GetBusStopList(stops =>
            {
                var nearbyStops = stops
					.OrderBy(s => RelativeDistance(s.Location, location))
					.Take(NumNearbyStops)
					.ToList();
                callback(nearbyStops);
            });
        }
        
        private static double RelativeDistance(GeographicCoordinate c1, GeographicCoordinate c2)
        {
            if (c1 == null || c2 == null)
            {
                return double.MaxValue;
            }

            var dlat = c1.Latitude - c2.Latitude;
            var dlon = c1.Longtitude - c2.Longtitude;

            return Math.Sqrt(dlat * dlat + dlon * dlon);
        }
				
		
		private List<BusStop> GetBusStopsFromDb()
		{
			return _conn.Table<BusStop>().ToList();			
		}

        private void AddStopsToDb(IEnumerable<BusStop> busStops)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                _conn.BeginTransaction();
                _conn.Table<BusStop>().ToList().ForEach(s => _conn.Delete(s));
                _conn.InsertAll(busStops);
                _conn.Commit();
            });
        }

        public void GetRealTimeData(BusStop busStop, Action<List<StopTime>> callback)
        {

            var client = GetClient();
            client.getUserRealTimeForecastCompleted += (s, e) =>
            {
                var stopTimes = new StopTimeConverter().GetStopTimes(e.Result);
                callback(stopTimes);
            };
            client.getUserRealTimeForecastAsync(Authentication, GetHandShake(), busStop.Id);
        }

		#region IBusStopRepository implementation
		public IList<BusStop> GetAll ()
		{
			throw new NotImplementedException ();
		}

		public IList<BusStop> GetFavorites ()
		{
			throw new NotImplementedException ();
		}

		public IList<BusStop> GetMostRecent ()
		{
			throw new NotImplementedException ();
		}

		public IList<BusStop> GetNearby ()
		{
			throw new NotImplementedException ();
		}

		public void AddMostRecent (BusStop busStop)
		{
			throw new NotImplementedException ();
		}

		public void AddFavorite (BusStop busStop)
		{
			throw new NotImplementedException ();
		}

		public void RemoveFavorite (BusStop busStop)
		{
			throw new NotImplementedException ();
		}
		#endregion
	
	}
}
