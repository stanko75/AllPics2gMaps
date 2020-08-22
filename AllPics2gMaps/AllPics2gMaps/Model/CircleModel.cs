namespace AllPics2gMaps.Model
{
  public class CircleModel
  {
    public float radius { get; set; }
    public float lat { get; set; }
    public float lng { get; set; }
  }

  public class Circles
  {
    public CircleModel[] circles { get; set; }
  }
}