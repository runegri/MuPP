
using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
namespace AtB
{
	public interface IBusStopRepository
	{
		IEnumerable<BusStop> GetAll();
		IEnumerable<BusStop> GetFavorites();
		IEnumerable<BusStop> GetMostRecent();
		IEnumerable<BusStop> GetNearby();
		
		void AddMostRecent(BusStop busStop);
		void AddFavorite(BusStop busStop);
		void RemoveFavorite(BusStop busStop);
	}	
}
