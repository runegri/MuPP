
using System;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;
namespace iPhoneUI
{
	public class BusStopRepository : IBusStopRepository
	{
		public IEnumerable<StopInfo> GetFavorites()
		{
			return new List<StopInfo> {new StopInfo("", "Fjøslia","","")};
		}
		
		public IEnumerable<StopInfo> GetMostRecent()
		{
			return new List<StopInfo> {new StopInfo("", "Fjøslia","","")};
		}
	}	
}
