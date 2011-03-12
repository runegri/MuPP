
using System;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;
namespace iPhoneUI
{
	public interface IBusStopRepository
	{
		IEnumerable<StopInfo> GetAll();
		IEnumerable<StopInfo> GetFavorites();
		IEnumerable<StopInfo> GetMostRecent();
		void AddMostRecent(StopInfo stopInfo);
	}	
}
