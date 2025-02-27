using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class NumberFieldBuilder : Builder<NumberField>
  {
    public NumberFieldBuilder(NumberField component) : base(component) { }







    public string XType => "NumberField";

  }
}
