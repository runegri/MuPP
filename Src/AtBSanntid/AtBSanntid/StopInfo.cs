using System;

namespace Holdeplasser
{
    public class StopInfo
    {

        public const int RegTopLineLength = 87;
        public const int StopNrStart = 4;
        public const int StopNrLength = 8;
        public const int StopNameStart = 12;
        public const int StopNameLength = 30;
        public const int XCoordStart = 53;
        public const int XCoordLength = 10;
        public const int YCoordStart = 63;
        public const int YCoordLength = 10;

        public StopInfo(string stopNumber, string stopName, string xCoord, string yCoord)
        {
            StopNumber = stopNumber;
            StopName = stopName;
            XCoord = xCoord;
            YCoord = yCoord;
        }

        public StopInfo(string regToppLine)
        {
            ParseRegTopp(regToppLine);
        }

        public string StopNumber { get; private set; }
        public string StopName { get; private set; }
        public string XCoord { get; private set; }
        public string YCoord { get; private set; }
        public GeographicCoordinate LatLon { get; private set; }

        private void ParseRegTopp(string regToppLine)
        {

            if(regToppLine.Length != RegTopLineLength)
            {
                throw new RegTopParseException(
                    "Invalid line length. Was " + regToppLine.Length + " expected " + RegTopLineLength);
            }

            StopNumber = regToppLine.Substring(StopNrStart, StopNrLength).Substring(4,4).Trim();
            StopName = StopNumber + " " + regToppLine.Substring(StopNameStart, StopNameLength).Trim();
            XCoord = regToppLine.Substring(XCoordStart, XCoordLength).Trim();
            YCoord = regToppLine.Substring(YCoordStart, YCoordLength).Trim();

            var utmX = double.Parse(XCoord);
            var utmY = double.Parse(YCoord);
            var utmCoordinate = new CartesianCoordinate(utmX, utmY);

            LatLon = new CoordinateConverter().UtmXyToLatLon(utmCoordinate, 32, false);
            
        }

        public override string ToString()
        {
            return "Name: " + StopName + "\t Nr: " + StopNumber + "\t LatLon: " + LatLon;
        }

        public class RegTopParseException : Exception
        {
            public RegTopParseException()
            {
            }

            public RegTopParseException(string message) : base(message)
            {
            }

        }

    }
}
