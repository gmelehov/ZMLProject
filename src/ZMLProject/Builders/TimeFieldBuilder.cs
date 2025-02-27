using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class TimeFieldBuilder : Builder<TimeField>
  {
    public TimeFieldBuilder(TimeField component) : base(component) { }







    public string XType => "TimeField";

  }
}
