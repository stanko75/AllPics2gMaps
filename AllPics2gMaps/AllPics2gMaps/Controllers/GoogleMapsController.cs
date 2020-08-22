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
  public class GoogleMapsController : ControllerBase
  {
    // GET: api/GoogleMaps
    [HttpGet]
    public string Get()
    {
      string groupedByCities = string.Empty;
      DB mySqlDB = new DB();

      try
      {
        mySqlDB.GpsMySqlQuery = "SELECT * FROM gpslocations INNER JOIN cities ON gpslocations.CityID = cities.ID GROUP BY cities.ID";
        mySqlDB.GpsMySqlConnection.Open();
        List<LatLngFileNameModel> latLngFileNames = new List<LatLngFileNameModel>();
        MySqlDataReader mySqlDataReader = mySqlDB.GpsMySqlDataReader;

        while (mySqlDataReader.Read())
        {
          LatLngFileNameModel latLngFileName = new LatLngFileNameModel
          {
            Latitude = mySqlDataReader["Latitude"].ToString(),
            Longitude = mySqlDataReader["Longitude"].ToString(),
            FileName = mySqlDataReader["FileName"].ToString()
          };

          string latitude = mySqlDataReader["Latitude"].ToString();
          latLngFileNames.Add(latLngFileName);
        }

        groupedByCities = JsonSerializer.Serialize(latLngFileNames);
      }
      finally
      {
        mySqlDB.Dispose();
      }

      return groupedByCities;
    }

    // POST: api/GoogleMaps
    [HttpPost]
    public ActionResult<string> Post([FromBody] Filter value)
    {
      string groupedByCities = string.Empty;
      string unionCitiesAndGpsLocations = string.Empty;
      DB mySqlDB = new DB();

      try
      {
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

        mySqlDB.GpsMySqlQuery = unionCitiesAndGpsLocations;
        mySqlDB.GpsMySqlConnection.Open();
        List<LatLngFileNameModel> latLngFileNames = new List<LatLngFileNameModel>();
        MySqlDataReader mySqlDataReader = mySqlDB.GpsMySqlDataReader;

        while (mySqlDataReader.Read())
        {
          LatLngFileNameModel latLngFileName = new LatLngFileNameModel
          {
            Latitude = mySqlDataReader["Latitude"].ToString(),
            Longitude = mySqlDataReader["Longitude"].ToString(),
            FileName = mySqlDataReader["FileName"].ToString()
          };

          latLngFileNames.Add(latLngFileName);
        }

        groupedByCities = JsonSerializer.Serialize(latLngFileNames);
      }
      finally
      {
        mySqlDB.Dispose();
      }

      return Ok(JsonSerializer.Serialize(groupedByCities));

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
