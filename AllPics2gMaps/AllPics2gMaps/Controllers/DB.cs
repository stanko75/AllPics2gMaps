using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;

namespace AllPics2gMaps.Controllers
{
  public class DB: IDisposable
  {
    public string GpsMySqlQuery { get; set; }
    private MySqlConnection m_mySqlConnection;
    private MySqlCommand m_mySqlCommand;

    public MySqlConnection GpsMySqlConnection {
      get
      {
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
        string connectionString = configuration["connectionString"];
        m_mySqlConnection = new MySqlConnection(connectionString);

        return m_mySqlConnection;
      }
    }

    private MySqlCommand GpsMySqlCommand
    {
      get
      {
        m_mySqlCommand = new MySqlCommand(GpsMySqlQuery, m_mySqlConnection);
        return m_mySqlCommand;
      }
    }  

    public MySqlDataReader GpsMySqlDataReader
    {
      get
      {
        return GpsMySqlCommand.ExecuteReader();
      }
    }

    public void Dispose()
    {
      m_mySqlCommand?.Dispose();
      m_mySqlConnection?.Dispose();
    }
  }
}