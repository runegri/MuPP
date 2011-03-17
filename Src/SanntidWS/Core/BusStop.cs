using SQLite;
using MonoTouch.Foundation;
using System;
namespace AtB
{
	
	[Preserve(AllMembers=true)]
    public class BusStop
    {
		
		[PrimaryKey, Indexed]
        public string Id { get; set; }

		public string StopCode { get; set; }
		
		[Indexed]
		public string Name { get; set; }
        
		[Ignore]
		public GeographicCoordinate Location { get; set; }
		
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
		
		[Indexed]
		public DateTime LastAccess { get; set; }
		
		[Indexed]
		public bool IsFavorite { get; set; }
		
		public BusStop()
		{
			Location = new GeographicCoordinate(0,0);
			LastAccess = DateTime.MinValue;
		}
		
        public BusStop(string id, string stopCode, string name, GeographicCoordinate location)
        {
            Id = id;
            StopCode = stopCode;
            Name = name;
            Location = location;
			LastAccess = DateTime.MinValue;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, Code: {2}, Loc: {3}", Id, Name, StopCode, Location);
        }

    }
}
