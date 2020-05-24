using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text.Json;

namespace AllPics2gMaps.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CitiesController : ControllerBase
  {
    // GET: api/Cities
    [HttpGet]
    public string Get()
    {
      string citiesJson = string.Empty;

      DB mySqlDB = new DB();

      try
      {
        mySqlDB.GpsMySqlQuery = "SELECT * FROM cities";
        mySqlDB.GpsMySqlConnection.Open();
        List<LatLngFileNameModel> latLngFileNames = new List<LatLngFileNameModel>();
        MySqlDataReader mySqlDataReader = mySqlDB.GpsMySqlDataReader;

        List<CityModel> cities = new List<CityModel>();
        while (mySqlDataReader.Read())
        {
          CityModel city = new CityModel
          {
            Name = mySqlDataReader["Name"].ToString()
          };

          cities.Add(city);
        }

        citiesJson = JsonSerializer.Serialize(cities);
      }
      finally
      {
        mySqlDB.Dispose();
      }

      return citiesJson;
    }
  }
}