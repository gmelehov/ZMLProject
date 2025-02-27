using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class MonthComboBuilder : Builder<MonthCombo>
  {
    public MonthComboBuilder(MonthCombo component) : base(component) { }







    public string XType => "MonthField";

  }
}
