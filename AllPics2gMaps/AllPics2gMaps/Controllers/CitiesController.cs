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
      string citiesJson;

      DB mySqlDb = new DB();

      try
      {
        mySqlDb.GpsMySqlQuery = "SELECT * FROM cities order by Name";
        mySqlDb.GpsMySqlConnection.Open();
        MySqlDataReader mySqlDataReader = mySqlDb.GpsMySqlDataReader;

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
        mySqlDb.Dispose();
      }

      return citiesJson;
    }
  }
}