using System.Collections.Generic;
using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Text.Json;

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

    // GET: api/GoogleMaps
    //[HttpGet]
    //public IEnumerable<string> Get()
    //{
    //  return new string[] { "value1", "value2" };
    //}

    // GET: api/GoogleMaps/5
    //[HttpGet("{id}", Name = "Get")]
    //public string Get(int id)
    //{
    //  return "value";
    //}

    [HttpGet("{json}", Name = "Get")]
    public string Get(string json)
    {
      return json;
    }

    // POST: api/GoogleMaps
    [HttpPost]
    public void Post([FromBody] string value)
    {
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
