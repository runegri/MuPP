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
using GpsTool;

namespace AtB
{
	
    public class BusStopRepository : IBusStopRepository
    {

        private const string UserName = "user";
        private const string Password = "pass";
        private const string UserId = "runegri";
		private const string DbName = "Sanntid.db";
		private const int NumNearbyStops = 8;
		private const int MostRecentCount = 8;
		
		private SQLiteConnection _conn;
		
		private readonly IGpsService _gpsService;

		private LocationData _location;
		
		public BusStopRepository(IGpsService gpsService)
		{
			try
			{
				CreateDatabaseFile();
				_gpsService = gpsService;
				_gpsService.LocationChanged = LocationChanged;
				_gpsService.Start();
			    ThreadPool.QueueUserWorkItem(o => 
                {
					GetHandShake();
					//GetBusStopList();
				});
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}
		
		private void CreateDatabaseFile()
		{
			var dbPath = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), DbName);
			//var dbPath = DbName;
			var sourceFile = "Db/sanntid.db";
			
			
			if(!File.Exists(dbPath))
			{
				File.Copy(sourceFile, dbPath);
			}
			
			_conn = new SQLiteConnection(dbPath);
			_conn.CreateTable<BusStop>();	
	
		}
		
		public void Reset()
		{
					
			_conn.Table<BusStop>().ToList().ForEach(b => {
				b.LastAccess = DateTime.MinValue;
				b.IsFavorite = false;
				_conn.Update(b);
			});
			
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
        
        public void GetBusStopList()
        {
			lock(_conn)
			{
				var stopsInDb = GetBusStopsFromDb();
				if (!stopsInDb.Any())
				{
				    var client = GetClient();
					client.GetBusStopsListCompleted += (s,e) =>
		            {
						var busStopsFromJson = new BusStopListConverter().GetBusStopsList(e.Result);
						AddStopsToDb(busStopsFromJson);
		            };
					client.GetBusStopsListAsync(Authentication);
				}
			}
		}
		

        private List<BusStop> GetNearbyStops(double latitude, double longtitude)
        {
			
			// Calculates the relative distance using the formula sqrt(dx*dx+dy*dy)
			// To optimize I don't care about the sqrt, so I only calculate dx*dx + dy*dy
			
			var sql = "SELECT * FROM BusStop " +
				"ORDER BY ((Latitude-?)*(Latitude-?) + (Longtitude-?)*(Longtitude-?)) " +
				"LIMIT 0, {0}";
			sql = string.Format(sql, NumNearbyStops);
			
			return _conn
				.Query<BusStop>(sql, latitude, latitude, longtitude, longtitude)
				.ToList();
		}	
		
		private List<BusStop> GetBusStopsFromDb()
		{
			lock(_conn)
			{
				return _conn.Table<BusStop>().OrderBy(s => s.Name).ToList();			
			}
		}

        private void AddStopsToDb(IEnumerable<BusStop> busStops)
        {
	        _conn.BeginTransaction();
	        _conn.Table<BusStop>().ToList().ForEach(s => _conn.Delete(s));
	        _conn.InsertAll(busStops);
	        _conn.Commit();
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
		
		private void LocationChanged(LocationData location)
		{
			_location = location;
		}

		#region IBusStopRepository implementation
		public IList<BusStop> GetAll ()
		{
			return GetBusStopsFromDb();
		}

		public IList<BusStop> GetFavorites ()
		{
			var sql = "SELECT * FROM BusStop WHERE IsFavorite = ? ORDER BY Name";
			return _conn.Query<BusStop>(sql, true).ToList();
		}
		
		public IList<BusStop> GetMostRecent ()
		{
			var sql = "SELECT * FROM BusStop WHERE LastAccess > ? ORDER BY LastAccess DESC LIMIT 0,{0}";	
			sql = string.Format(sql, MostRecentCount);
			return _conn.Query<BusStop>(sql, DateTime.MinValue).ToList();
		}

		public IList<BusStop> GetNearby ()
		{
			if(_location != null)
			{
				return GetNearbyStops(_location.Latitude, _location.Longtitude);
			}
			else
			{
				return new List<BusStop>();
			}
		}

		public void AddMostRecent (BusStop busStop)
		{
			busStop.LastAccess = DateTime.Now;
			_conn.Update(busStop);			
		}

		public void AddFavorite (BusStop busStop)
		{
			busStop.IsFavorite = true;
			_conn.Update(busStop);
		}

		public void RemoveFavorite (BusStop busStop)
		{
			busStop.IsFavorite = false;
			_conn.Update(busStop);
		}
		
		#endregion
	
	}
}
