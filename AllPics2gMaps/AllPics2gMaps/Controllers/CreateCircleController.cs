using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace AllPics2gMaps.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CreateCircleController : ControllerBase
  {
    [HttpPost]
    public ActionResult<string> Post([FromBody] Circles value)
    {
      string groupedByCircles = string.Empty;
      string sqlTemplate = "SELECT "
                              + "*, ( "
                              + "{0} * acos( "
                              + "cos(radians({1})) "
                              + "* cos(radians(latitude)) "
                              + "* cos(radians(longitude) - radians({2})) "
                              + "+ sin(radians({1})) "
                              + "* sin(radians(latitude)) "
                              + "   )"
                              + ") AS distance "
                              + "FROM gpslocations "
                              + "ORDER BY distance "
                              + "LIMIT 0 , 20;";
      string unionCircles = string.Empty;

      if (value.circles.Length > 0)
      {
        foreach (CircleModel circle in value.circles)
        {
          if (string.IsNullOrWhiteSpace(unionCircles))
          {
            unionCircles = string.Format(sqlTemplate
              , circle.radius.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })
              , circle.lat.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })
              , circle.lng.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })
             );
          }
          else
          {
            unionCircles = unionCircles
              + " UNION ALL "
              + string.Format(sqlTemplate, circle.radius, circle.lat, circle.lng);
          }
        }
      }

      using (DB mySqlDB = new DB())
      {
        mySqlDB.GpsMySqlQuery = unionCircles;
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

        groupedByCircles = JsonSerializer.Serialize(latLngFileNames);
      }

      return Ok(JsonSerializer.Serialize(groupedByCircles));
    }

  }
}
