namespace CpApi.Resource
{
	public class RouteResource
	{
		public int Id { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

		public string TotalRouteKm { get; set; }

		public string TotalRouteTime { get; set; }

		public List<LineResource> LineResources { get; set; }

		public RouteResource()
		{
			LineResources = new List<LineResource>();
		}
	}
}
