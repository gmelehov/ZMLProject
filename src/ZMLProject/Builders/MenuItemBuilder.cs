using DiffCode.JSGenerator;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class MenuItemBuilder : Builder<MenuItem>
  {
    public MenuItemBuilder(MenuItem component) : base(component) { }






    public string IconCls => Self["icon"] + (Self["color"] != null ? (" " + Self["color"]) : "''");

    public string Cls => "logx mat-icons";






    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("handler", "call")]
    public KeyValue OnClick => Self.OnEvent($"handler", $"call");

  }
}
