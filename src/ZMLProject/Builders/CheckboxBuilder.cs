using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class CheckboxBuilder : Builder<Checkbox>
  {
    public CheckboxBuilder(Checkbox component) : base(component) { }







    public string XType => "Checkbox";

  }
}
