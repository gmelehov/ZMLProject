using ZMLProject.Models.Dockable;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class ToolbarBuilder : Builder<Toolbar>
  {
    public ToolbarBuilder(Toolbar component) : base(component) { }







    public string XType => "toolbar";

  }
}
