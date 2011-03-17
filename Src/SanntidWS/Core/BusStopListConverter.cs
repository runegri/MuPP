using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AtB
{
    internal class BusStopListConverter : JsonConverterBase
    {

        public List<BusStop> GetBusStopsList(string busStopJson)
        {

            var jObject = JObject.Parse(busStopJson);

            var stops = new List<BusStop>();
            foreach (var stop in jObject.SelectToken("Fermate"))
            {
                var id = GetTokenValue(stop.SelectToken("cinFermata"));
                var stopCode = GetTokenValue(stop.SelectToken("codAzNodo"));
                var name = GetTokenValue(stop.SelectToken("descrizione"));
				var lat = Convert.ToDouble(GetTokenValue(stop.SelectToken("lat")));
                var lon = Convert.ToDouble(GetTokenValue(stop.SelectToken("lon")));

                name = name.Replace(stopCode, "").Replace("()", "").Trim();
				name = name + " " + DirectionFromStopCode(stopCode);
				
                var location = ConvertWSCoordinates(lat, lon);

                var busStop = new BusStop(id, stopCode, name, location);
                stops.Add(busStop);
            }

            return stops;

        }
		
		private string DirectionFromStopCode(string stopCode)
		{
			// The stop code is a string with 8 digits
			// The fifth determines if the direction towards the city center of from
			if(stopCode[4].Equals('1'))
			{
				return "(mot sentrum)";
			}
			else
			{
				return "(fra sentrum)";
			}
		}
		
		/// <summary>
		/// Hack for converting coordinates from the web service to latitude/longtitude.
		/// Accurate down to 1m-15m which is good enough for us here. 
		/// </summary>
		/// <param name="lat">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <param name="lon">
		/// A <see cref="System.Double"/>
		/// </param>
		/// <returns>
		/// A <see cref="GeographicCoordinate"/>
		/// </returns>
        private static GeographicCoordinate ConvertWSCoordinates(double lat, double lon)
        {
            var corrLat = (lat + 6541352)/248270.79;
            var corrLon = lon/111319.4447;
            return new GeographicCoordinate(corrLat, corrLon);
        }

    }
}
