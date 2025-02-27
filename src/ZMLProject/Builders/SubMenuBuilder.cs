namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class SubMenuBuilder : Builder<SubMenu>
  {
    public SubMenuBuilder(SubMenu component) : base(component) { }









    public string Cls => "logx mat-icons";

    public string IconCls => Self.Icon + (Self.Color != null ? (" " + Self.Color) : "");

  }
}
