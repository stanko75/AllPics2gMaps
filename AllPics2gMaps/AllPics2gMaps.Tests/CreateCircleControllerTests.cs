using AllPics2gMaps.Controllers;
using AllPics2gMaps.Model;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace AllPics2gMaps.Tests
{
  public class CreateCircleControllerTests
  {
    [Test]
    public void CheckPostMethod()
    {
      CirclesClass value = new CirclesClass();

      value.Circles = new CircleModel[1];
      value.Circles[0] = new CircleModel();

      value.Circles[0].Lat = 58.3758354F;
      value.Circles[0].Lng = 97.0307159F;
      value.Circles[0].Radius = 100000;

      CreateCircleController createCircleController = new CreateCircleController();
      ActionResult<string> circles = createCircleController.Post(value);
      Assert.IsNotNull(circles.Result);
      string resultValue = (string)((ObjectResult)circles.Result).Value;
      Assert.IsFalse(string.IsNullOrWhiteSpace(resultValue));

    }
  }
}