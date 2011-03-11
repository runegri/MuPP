
using System;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;
namespace iPhoneUI
{
	public class BusStopRepository : IBusStopRepository
	{
		private List<StopInfo> _mostRecent = new List<StopInfo>();
		
		public IEnumerable<StopInfo> GetAll()
		{
			var vm = new HoldeplasserViewModel();
			return vm.Stops;
		}

		public IEnumerable<StopInfo> GetFavorites()
		{
			return new List<StopInfo> {new StopInfo("", "Fjøslia","","")};
		}
		
		public IEnumerable<StopInfo> GetMostRecent()
		{
			return _mostRecent;
		}
		
		public void AddMostRecent(StopInfo stopInfo) 
		{
			_mostRecent.Add(stopInfo);
		}
	}	
}
