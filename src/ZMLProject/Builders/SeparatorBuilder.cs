using ZMLProject.Models.Dockable;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class SeparatorBuilder : Builder<Separator>
  {
    public SeparatorBuilder(Separator component) : base(component) { }







    public string XType => "tbseparator";

  }
}
