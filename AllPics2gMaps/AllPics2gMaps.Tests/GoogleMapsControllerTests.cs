using AllPics2gMaps.Controllers;
using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace AllPics2gMaps.Tests
{
  public class GoogleMapsControllerTests
  {
    GoogleMapsController googleMapsController;

    [SetUp]
    public void Setup()
    {
      googleMapsController = new GoogleMapsController();
    }

    [Test]
    public void CheckIfGetMethodReturnsAnything()
    {
      string googleMaps = googleMapsController.Get();

      Assert.IsFalse(string.IsNullOrWhiteSpace(googleMaps));
    }

    [Test]
    public void CheckPostMethodFilter()
    {
      GoogleMapsFilterModel value = new GoogleMapsFilterModel();

      value.Cities = new string[1];
      value.Cities[0] = "Bonn";
      value.Limit = 5;

      ActionResult<string> googleMaps = googleMapsController.Post(value);
      string resultValue = (string)((ObjectResult)googleMaps.Result).Value;
      Assert.IsNotNull(googleMaps.Result);
      Assert.IsFalse(string.IsNullOrWhiteSpace(resultValue));

      dynamic message = JsonConvert.DeserializeObject(resultValue);
      JArray jArray = JArray.Parse(message);
      Assert.IsTrue(jArray.Count > 0);
    }
  }
}