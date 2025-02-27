using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class DateFieldBuilder : Builder<DateField>
  {
    public DateFieldBuilder(DateField component) : base(component) { }







    public string XType => "DateField";

  }
}
