namespace AtB
{
    public class BusStop
    {
        public string Id { get; private set; }
        public string StopCode { get; private set; }
        public string Name { get; private set; }
        public GeographicCoordinate Location { get; private set; }

        public BusStop(string id, string stopCode, string name, GeographicCoordinate location)
        {
            Id = id;
            StopCode = stopCode;
            Name = name;
            Location = location;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Code: {2}, Loc: {3}", Id, Name, StopCode, Location);
        }

    }
}
