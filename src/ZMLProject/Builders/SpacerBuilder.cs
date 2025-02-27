using ZMLProject.Models.Dockable;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class SpacerBuilder : Builder<Spacer>
  {
    public SpacerBuilder(Spacer component) : base(component) { }







    public string XType => "tbspacer";

  }
}
