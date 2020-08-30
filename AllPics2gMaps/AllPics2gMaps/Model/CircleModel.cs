namespace AllPics2gMaps.Model
{
  public class CircleModel
  {
    public float Radius { get; set; }
    public float Lat { get; set; }
    public float Lng { get; set; }
  }

  public class CirclesClass
  {
    public CircleModel[] Circles { get; set; }
  }
}