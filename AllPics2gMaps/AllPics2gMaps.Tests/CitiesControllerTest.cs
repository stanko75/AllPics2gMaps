using AllPics2gMaps.Controllers;
using NUnit.Framework;

namespace AllPics2gMaps.Tests
{
  public class Tests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CheckIfGetCitiesReturnsAnything()
    {
      CitiesController citiesController = new CitiesController();
      string cities = citiesController.Get();

      Assert.IsFalse(string.IsNullOrWhiteSpace(cities));
    }
  }
}