using System.Collections.Generic;
using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Text.Json;
using System.Threading.Tasks;

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
    public ActionResult<string> Post([FromBody] Filter value)
    {
      string unionCitiesAndGpsLocations = string.Empty;

      if (value.cities.Length > 0)
      {
        string sqlTemplate = "("
            + "SELECT gpslocations.* FROM cities "
            + "INNER JOIN gpslocations ON gpslocations.CityID = cities.ID "
            + "WHERE cities.Name = '{0}' "
            + "LIMIT {1}"
            + ")";

        foreach (string city in value.cities)
        {
          if (string.IsNullOrWhiteSpace(unionCitiesAndGpsLocations))
          {
            unionCitiesAndGpsLocations = string.Format(sqlTemplate, city, value.limit);
          }
          else
          {
            unionCitiesAndGpsLocations = unionCitiesAndGpsLocations
              + " UNION ALL "
              + string.Format(sqlTemplate, city, value.limit);
          }
        }
      }

      return Ok(JsonSerializer.Serialize(GetLatLngFromDB(unionCitiesAndGpsLocations)));
    }

    public class Filter
    {
      public string[] cities { get; set; }
      public int limit { get; set; }
    }

    // PUT: api/GoogleMaps/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
