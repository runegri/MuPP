// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace AtBSanntid {
	
	
	// Base type probably should be MonoTouch.Foundation.NSObject or subclass
	[MonoTouch.Foundation.Register("AppDelegate")]
	public partial class AppDelegate {
		
		private MonoTouch.UIKit.UIWindow __mt_window;
		
		private MonoTouch.UIKit.UITextField __mt_BusStopCode;
		
		private MonoTouch.MapKit.MKMapView __mt_BusStopMap;
		
		private MonoTouch.UIKit.UIActivityIndicatorView __mt_ActivityIndicator;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("window")]
		private MonoTouch.UIKit.UIWindow window {
			get {
				this.__mt_window = ((MonoTouch.UIKit.UIWindow)(this.GetNativeField("window")));
				return this.__mt_window;
			}
			set {
				this.__mt_window = value;
				this.SetNativeField("window", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("BusStopCode")]
		private MonoTouch.UIKit.UITextField BusStopCode {
			get {
				this.__mt_BusStopCode = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("BusStopCode")));
				return this.__mt_BusStopCode;
			}
			set {
				this.__mt_BusStopCode = value;
				this.SetNativeField("BusStopCode", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("BusStopMap")]
		private MonoTouch.MapKit.MKMapView BusStopMap {
			get {
				this.__mt_BusStopMap = ((MonoTouch.MapKit.MKMapView)(this.GetNativeField("BusStopMap")));
				return this.__mt_BusStopMap;
			}
			set {
				this.__mt_BusStopMap = value;
				this.SetNativeField("BusStopMap", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("ActivityIndicator")]
		private MonoTouch.UIKit.UIActivityIndicatorView ActivityIndicator {
			get {
				this.__mt_ActivityIndicator = ((MonoTouch.UIKit.UIActivityIndicatorView)(this.GetNativeField("ActivityIndicator")));
				return this.__mt_ActivityIndicator;
			}
			set {
				this.__mt_ActivityIndicator = value;
				this.SetNativeField("ActivityIndicator", value);
			}
		}
	}
}
