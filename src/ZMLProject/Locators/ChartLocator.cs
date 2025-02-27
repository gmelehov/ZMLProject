using ZMLProject.Models.Charts;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class ChartLocator : Locator<Chart>
  {
    public ChartLocator(string glob, string app, string id) : base(glob, app, id) { }
















    public override string Manager => "Chart";
  }
}
