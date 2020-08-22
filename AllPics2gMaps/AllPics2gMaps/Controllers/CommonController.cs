using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Text.Json;

namespace AllPics2gMaps.Controllers
{
  public class CommonController : ControllerBase
  {
    public string GetLatLngFromDB(string sql)
    {
        string groupedByCircles = string.Empty;
        using (DB mySqlDB = new DB())
        {
          mySqlDB.GpsMySqlQuery = sql;
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

        return groupedByCircles;
    }
  }
}
