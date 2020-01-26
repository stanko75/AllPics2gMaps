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
      IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
      string connectionString = configuration["connectionString"];
      string mySQL = "SELECT * FROM gpslocations INNER JOIN cities ON gpslocations.CityID = cities.ID GROUP BY cities.ID";

      using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
      {
        using (MySqlCommand mySqlCommand = new MySqlCommand(mySQL, mySqlConnection))
        {
          mySqlConnection.Open();

          MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

          List<LatLngFileNameModel> latLngFileNames = new List<LatLngFileNameModel>();

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
    [HttpGet("{id}", Name = "Get")]
    public string Get(int id)
    {
      return "value";
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
