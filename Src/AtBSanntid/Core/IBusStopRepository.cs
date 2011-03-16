
using System;
using MonoTouch.UIKit;
using Core;
using System.Collections.Generic;
namespace iPhoneUI
{
	public interface IBusStopRepository
	{
		IEnumerable<StopInfo> GetAll();
		
		IList<StopInfo> GetFavorites();
		void AddFavorite(StopInfo stopInfo);
		
		IList<StopInfo> GetMostRecent();
		void AddMostRecent(StopInfo stopInfo);
	}	
}
