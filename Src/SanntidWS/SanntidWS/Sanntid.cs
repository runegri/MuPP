using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace AtB
{
    public interface ISanntidData
    {
        void GetBusStopList(Action<List<BusStop>> callback);
        void GetRealTimeData(BusStop busStop, Action<List<StopTime>> callback);
    }

    public class Sanntid : ISanntidData
    {

        private const string UserName = "user";
        private const string Password = "pass";
        private const string UserId = "runegri";
		
        private HandShake _handShake;
        private HandShake GetHandShake()
        {
            if (_handShake == null)
            {
				_handShake = GetClient().getToken(Authentication, UserId);
            }
            return _handShake;
		}

        private UserServices GetClient()
        {	
            return new UserServices(); //WsBinding, EndPoint);
        }

        private static Binding WsBinding
        {
            get
            {
                var binding = new BasicHttpBinding { MaxReceivedMessageSize = 4096 * 1024, MaxBufferSize = 4096 * 1024 };
                binding.ReaderQuotas.MaxStringContentLength = 4096 * 1024;
                return binding;
            }
        }

        private static EndpointAddress EndPoint
        {
            get { return new EndpointAddress("http://195.0.188.74/InfoTransit/userservices.asmx"); }
        }

        private static WsAuthentication Authentication
        {
            get { return new WsAuthentication { user = UserName, password = Password }; }
        }
        
        public void GetBusStopList(Action<List<BusStop>> callback)
        {
            var client = GetClient();
			client.GetBusStopsListCompleted += (s,e) =>
            {
                var json = e.Result; //.Body.GetBusStopsListResult;
                callback(new BusStopListConverter().GetBusStopsList(json));
            };
			client.GetBusStopsListAsync(Authentication);
			
		}

        public void GetRealTimeData(BusStop busStop, Action<List<StopTime>> callback)
        {

            var client = GetClient();
//            var request = new getUserRealTimeForecastRequest(
  //                          new getUserRealTimeForecastRequestBody(Authentication, GetHandShake(), busStop.Id));
            client.getUserRealTimeForecastCompleted += (s, e) =>
            {
                var json = e.Result; //.Body.getUserRealTimeForecastResult;
                var stopTimes = new StopTimeConverter().GetStopTimes(json);
                callback(stopTimes);
            };
            client.getUserRealTimeForecastAsync(Authentication, GetHandShake(), busStop.Id);
        }


    }
}
