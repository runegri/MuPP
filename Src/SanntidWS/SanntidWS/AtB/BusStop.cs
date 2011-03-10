using SQLite;
using MonoTouch.Foundation;
namespace AtB
{
	
	[Preserve(AllMembers=true)]
    public class BusStop
    {
		
		[PrimaryKey, Indexed]
        public string Id { get; private set; }

		public string StopCode { get; private set; }
		
		[Indexed]
		public string Name { get; private set; }
        
		[Ignore]
		public GeographicCoordinate Location { get; private set; }
		
		public double Latitude 
		{
			get { return Location.Latitude; }
			set { Location.Latitude = value; }
		}
		
		public double Longtitude
		{
			get { return Location.Longtitude;; }
			set { Location.Longtitude = value; }
		}
		
		public BusStop()
		{
			Location = new GeographicCoordinate(0,0);
		}
		
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
