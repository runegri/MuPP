using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AtB
{
    internal class StopTimeConverter:JsonConverterBase
    {

        public List<StopTime> GetStopTimes(string stopTimeJson)
        {
            var jObject = JObject.Parse(stopTimeJson);
			
			var times = jObject.SelectToken("Orari");
			if (times != null && times.Any())
			{
				return times.Select(token => StopTimeFromToken(token)).ToList();
			}
			else
			{
				return new List<StopTime>();
			}
		}

        private StopTime StopTimeFromToken(JToken token)
        {
            var routeNr = GetTokenValue(token.SelectToken("codAzLinea"));
            var routeName =  GetTokenValue(token.SelectToken("descrizioneLinea"));
            var routeTime = GetTokenValue(token.SelectToken("orario"));
            var timeType =  GetTokenValue(token.SelectToken("statoPrevisione"));

            return new StopTime(routeNr, routeName, routeTime, timeType);
        }

    }
}
