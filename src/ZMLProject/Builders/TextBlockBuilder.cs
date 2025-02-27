using ZMLProject.Models.Dockable;




namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class TextBlockBuilder : Builder<TextBlock>
  {
    public TextBlockBuilder(TextBlock component) : base(component) { }







    public string XType => "tbtext";

  }
}
