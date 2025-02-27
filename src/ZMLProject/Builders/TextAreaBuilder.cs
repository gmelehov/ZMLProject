using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class TextAreaBuilder : Builder<TextArea>
  {
    public TextAreaBuilder(TextArea component) : base(component) { }







    public string XType => "TextArea";

  }
}
