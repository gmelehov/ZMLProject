using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class TextFieldBuilder : Builder<TextField>
  {
    public TextFieldBuilder(TextField component) : base(component) { }







    public string XType => "TextField";

  }
}
