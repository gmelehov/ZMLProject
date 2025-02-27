namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class RestSourceBuilder : Builder<RestSource>
  {
    public RestSourceBuilder(RestSource component) : base(component) { }





    public string Extend => $"ZML.data.RestStore";

  }
}
