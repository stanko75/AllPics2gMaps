using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
      IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
      string connectionString = configuration["connectionString"];
      string mySQL = "SELECT * FROM cities";

      using (MySqlConnection mySqlConnection = new MySqlConnection(connectionString))
      {
        using (MySqlCommand mySqlCommand = new MySqlCommand(mySQL, mySqlConnection))
        {
          mySqlConnection.Open();
          List<CityModel> cities = new List<CityModel>();

          MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
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
      }

      return citiesJson;
    }
  }
}