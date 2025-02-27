namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class RestProxyBuilder : Builder<RestProxy>
  {
    public RestProxyBuilder(RestProxy component) : base(component) { }






    public string Type => "RESTProxy";

  }
}
