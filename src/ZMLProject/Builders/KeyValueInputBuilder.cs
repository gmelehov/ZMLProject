using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class KeyValueInputBuilder : Builder<KeyValueInput>
  {
    public KeyValueInputBuilder(KeyValueInput component) : base(component) { }








    public string XType => "KeyValueInput";

  }
}
