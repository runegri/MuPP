using System;
using System.Linq;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;
namespace iPhoneUI
{
	public class BusStopRepository : IBusStopRepository
	{
		private static List<StopInfo> _mostRecent = new List<StopInfo>();
		private static List<StopInfo> _favorites = new List<StopInfo>();
		
		public IEnumerable<StopInfo> GetAll()
		{
			var vm = new HoldeplasserViewModel();
			return vm.Stops;
		}

		public IList<StopInfo> GetFavorites()
		{
			return _favorites.OrderBy(f => f.StopName).ToList();
		}
		
		public void AddFavorite(StopInfo stopInfo) 
		{
			_favorites.Add(stopInfo);
		}
		
		public IList<StopInfo> GetMostRecent()
		{
			return _mostRecent;
		}
		
		public void AddMostRecent(StopInfo stopInfo) 
		{
			_mostRecent.Add(stopInfo);
		}
	}	
}
