using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AtB
{
    internal class BusStopListConverter : JsonConverterBase
    {

        private const int RegTopLineLength = 87;
        private const int StopNrStart = 4;
        private const int StopNrLength = 8;
        private const int XCoordStart = 53;
        private const int XCoordLength = 10;
        private const int YCoordStart = 63;
        private const int YCoordLength = 10;

        private readonly Dictionary<string, GeographicCoordinate> _stopCoordinates = new Dictionary<string, GeographicCoordinate>();
        private readonly CoordinateConverter _coordinateConverter = new CoordinateConverter();

        public List<BusStop> GetBusStopsList(string busStopJson)
        {

            using (var regToppData = GetType().Assembly.GetManifestResourceStream("SanntidWS.Data.R1615.HPL"))
            {
                ParseRegToppFile(regToppData);
            }

            var jObject = JObject.Parse(busStopJson);

            var stops = new List<BusStop>();
            foreach (var stop in jObject.SelectToken("Fermate"))
            {
                var id = GetTokenValue(stop.SelectToken("cinFermata"));
                var stopCode = GetTokenValue(stop.SelectToken("codAzNodo"));
                var name = GetTokenValue(stop.SelectToken("descrizione"));

                name = name.Replace(stopCode, "").Replace("()", "").Trim();

                GeographicCoordinate location;
                _stopCoordinates.TryGetValue(stopCode, out location);
                var busStop = new BusStop(id, stopCode, name, location);
                stops.Add(busStop);
            }

            return stops;

        }

        private void ParseRegToppFile(Stream regTopData)
        {
            using (var textReader = new StreamReader(regTopData))
            {
                while (!textReader.EndOfStream)
                {
                    var line = textReader.ReadLine();
                    ParseRegToppLine(line);
                }
            }
        }

        private void ParseRegToppLine(string regToppLine)
        {

            if (regToppLine.Length != RegTopLineLength)
            {
                throw new RegTopParseException(
                    "Invalid line length. Was " + regToppLine.Length + " expected " + RegTopLineLength);
            }

            var stopNumber = regToppLine.Substring(StopNrStart, StopNrLength).Trim();
            var xCoord = regToppLine.Substring(XCoordStart, XCoordLength).Trim();
            var yCoord = regToppLine.Substring(YCoordStart, YCoordLength).Trim();

            var utmX = double.Parse(xCoord);
            var utmY = double.Parse(yCoord);
            var utmCoordinate = new CartecianCoordinate(utmX, utmY);

            var coordinate = _coordinateConverter.UtmXyToLatLon(utmCoordinate, 32, false);

            _stopCoordinates.Add(stopNumber, coordinate);

        }

        public class RegTopParseException : Exception
        {
            public RegTopParseException()
            {
            }

            public RegTopParseException(string message)
                : base(message)
            {
            }

        }


    }
}
