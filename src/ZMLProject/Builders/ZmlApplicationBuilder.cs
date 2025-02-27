namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class ZmlApplicationBuilder : Builder<ZmlApplication>
  {
    public ZmlApplicationBuilder(ZmlApplication component) : base(component) { }









    public string GlobalVar => Self["globalVar"] ?? "GLOBAL";

    public string Name => Self["name"] ?? "APP";

  }
}
