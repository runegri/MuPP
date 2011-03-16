using System;
using GpsTool;
using MonoTouch.Foundation;
namespace GpsTool
{
	public class DebugGpsService : IGpsService
	{
		
		NSTimer _timer;
		bool _running;
		
		public DebugGpsService ()
		{
			_timer = NSTimer.CreateRepeatingScheduledTimer(1, TimerEvent);
		}
		
		private void TimerEvent()
		{
			if(_running)
			{
				var loc = new LocationData(63.425630295, 10.4458852325, 0, 1, DateTime.Now);
				if(LocationChanged != null)
				{
					LocationChanged(loc);
				}
			}
		}
		
		#region IGpsService implementation
		public void Start ()
		{
			_running = true;
		}

		public void Stop ()
		{
			_running = false;
		}

		public Action<LocationData> LocationChanged {
			get; set;
		}
		#endregion
}
}

