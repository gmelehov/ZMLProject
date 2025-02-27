namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class LocalSourceBuilder : Builder<LocalSource>
  {
    public LocalSourceBuilder(LocalSource component) : base(component) { }





    public string Extend => $"ZML.data.RestStore";

  }
}
