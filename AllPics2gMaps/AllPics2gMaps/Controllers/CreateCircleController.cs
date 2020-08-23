using System.Globalization;
using System.Text.Json;
using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;

namespace AllPics2gMaps.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CreateCircleController : CommonController
  {
    [HttpPost]
    public ActionResult<string> Post([FromBody] Circles value)
    {
      //query taken from https://developers.google.com/maps/solutions/store-locator/clothing-store-locator#findnearsql
      string sqlTemplate = "(SELECT "
                              + "*, ( "
                              + "6371 * acos( "
                              + "cos(radians({1})) "
                              + "* cos(radians(latitude)) "
                              + "* cos(radians(longitude) - radians({2})) "
                              + "+ sin(radians({1})) "
                              + "* sin(radians(latitude)) "
                              + "   )"
                              + ") AS distance "
                              + "FROM gpslocations "
                              + "HAVING distance < {0} "
                              + "ORDER BY distance "
                              + "LIMIT 0 , 20)";
      string unionCircles = string.Empty;

      if (value.circles.Length > 0)
      {
        foreach (CircleModel circle in value.circles)
        {
          float radius = circle.radius / 1000;

          string sql = string.Format(sqlTemplate
              , radius.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })
              , circle.lat.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })
              , circle.lng.ToString(new NumberFormatInfo() { NumberDecimalSeparator = "." })
             );

          if (string.IsNullOrWhiteSpace(unionCircles))
          {
            unionCircles = sql;
          }
          else
          {
            unionCircles = unionCircles
              + " UNION ALL "
              + sql;
          }
        }
        return Ok(JsonSerializer.Serialize(GetLatLngFromDB(unionCircles)));
      }

      return Ok();
    }

  }
}
