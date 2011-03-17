
using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
namespace AtB
{
	public interface IBusStopRepository
	{
		IList<BusStop> GetAll();
		IList<BusStop> GetFavorites();
		IList<BusStop> GetMostRecent();
		IList<BusStop> GetNearby();
		
		void AddMostRecent(BusStop busStop);
		void AddFavorite(BusStop busStop);
		void RemoveFavorite(BusStop busStop);
	
		void GetRealTimeData(BusStop busStop, Action<List<StopTime>> callback);
	}	
}
