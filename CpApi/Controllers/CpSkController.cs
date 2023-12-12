using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AngleSharp;
using AngleSharp.Dom;
using CpApi.Resource;


namespace CpApi.Controllers
{
	[ApiController]
	[Route("/api")]
	public class CpSkController : ControllerBase
	{	
		/// <summary>
		/// Funkcia vrati najblizsie 3 spoje
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="vehicle"></param>
		/// <returns></returns>
		[HttpGet("getRoutes")]
		[AllowAnonymous]
		public async Task<IActionResult> GetRoutes(string from, string to, string date = "", string time= "", string vehicle = "vlakbus")
		{
			List<RouteResource> routeResources = new List<RouteResource>();			

			if (date=="")
				date = DateTime.Now.ToString("dd.MM.yyyy");

			if (time == "")
				time = DateTime.Now.ToString("HH:mm");

            var config = Configuration.Default.WithDefaultLoader();
			string address = $"https://cp.hnonline.sk/{vehicle}/spojenie/vysledky/?date={date}&time={time}&f={from}&t={to}";
			using var context = BrowsingContext.New(config);
			IDocument doc = await context.OpenAsync(address);

			//var html = System.IO.File.ReadAllText(@"c:\TEMP\CP.html");						
			
			var connections = doc.QuerySelectorAll("div.box.connection");			

			foreach (var connection in connections)
			{
				RouteResource routeResource = new RouteResource();
				var outsideofGroup = connection.QuerySelectorAll("div.outside-of-popup");
				routeResource.Id = routeResources.Count + 1;

				if (connection.QuerySelector("div.date-total h2.reset.date") is not null)
				{
					if (connection.QuerySelector("div.date-total h2.reset.date") is not null)
						routeResource.Time = connection.QuerySelector("div.date-total h2.reset.date").TextContent.Substring(0, 5);

					if (connection.QuerySelector("div.date-total h2.reset.date span.date-after") is not null)
						routeResource.Date = connection.QuerySelector("div.date-total h2.reset.date span.date-after").TextContent;					
				}
					
				var routeinfo = connection.QuerySelectorAll("p.reset.total strong");
				
				routeResource.TotalRouteTime = routeinfo[0].TextContent;
				routeResource.TotalRouteKm = routeinfo[1].TextContent;

				foreach (var outsideof in outsideofGroup)
				{
					LineResource lineResource = new LineResource();
					if (outsideof.QuerySelector("span.owner") is not null)
						lineResource.Owner = outsideof.QuerySelector("span.owner").TextContent;

					if (outsideof.QuerySelector("div.title-container span") is not null)
						lineResource.VehicleNumber = outsideof.QuerySelector("div.title-container span").TextContent.Trim();

					if (outsideof.QuerySelector("span[class^=\"conn-result-delay-bubble\"]") is not null)
						lineResource.Delay = outsideof.QuerySelector("span[class^=\"conn-result-delay-bubble\"]").TextContent.Trim();

					if (outsideof.QuerySelector("li.item.active p.reset.time") is not null)
						lineResource.DepartureTime= outsideof.QuerySelector("li.item.active p.reset.time").TextContent;

					if (outsideof.QuerySelector("li.item.active p.station") is not null)
						lineResource.Boardingstation = outsideof.QuerySelector("li.item.active p.station").TextContent;

					if (outsideof.QuerySelector("li.item.active.last p.reset.time") is not null)
						lineResource.ArrivalTime = outsideof.QuerySelector("li.item.active.last p.reset.time").TextContent;

					if (outsideof.QuerySelector("li.item.active.last p.station") is not null)
						lineResource.ExitStation = outsideof.QuerySelector("li.item.active.last p.station").TextContent;

					if (outsideof.QuerySelector("div.walk") is not null)
						lineResource.Walk = outsideof.QuerySelector("div.walk").TextContent.Trim();

					routeResource.LineResources.Add(lineResource);	
				}

				routeResources.Add(routeResource);
				
			}

			return Ok(routeResources);
		}
	}
}