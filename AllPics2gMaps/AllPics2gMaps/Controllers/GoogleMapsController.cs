using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AllPics2gMaps.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GoogleMapsController : CommonController
  {
    // GET: api/GoogleMaps
    [HttpGet]
    public string Get()
    {
      return GetLatLngFromDB("SELECT * FROM gpslocations INNER JOIN cities ON gpslocations.CityID = cities.ID GROUP BY cities.ID");
    }

    // POST: api/GoogleMaps
    [HttpPost]
    public ActionResult<string> Post([FromBody] GoogleMapsFilterModel value)
    {
      string unionCitiesAndGpsLocations = string.Empty;

      if (value.Cities.Length > 0)
      {
        string sqlTemplate = "("
            + "SELECT gpslocations.* FROM cities "
            + "INNER JOIN gpslocations ON gpslocations.CityID = cities.ID "
            + "WHERE cities.Name = '{0}' "
            + "LIMIT {1}"
            + ")";

        foreach (string city in value.Cities)
        {
          if (string.IsNullOrWhiteSpace(unionCitiesAndGpsLocations))
          {
            unionCitiesAndGpsLocations = string.Format(sqlTemplate, city, value.Limit);
          }
          else
          {
            unionCitiesAndGpsLocations = unionCitiesAndGpsLocations
              + " UNION ALL "
              + string.Format(sqlTemplate, city, value.Limit);
          }
        }
      }

      return Ok(JsonSerializer.Serialize(GetLatLngFromDB(unionCitiesAndGpsLocations)));
    }
  }
}