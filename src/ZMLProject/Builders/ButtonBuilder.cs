using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Dockable;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class ButtonBuilder : Builder<Button>
  {
    public ButtonBuilder(Button component) : base(component) { }





    public string XType => "Button";

    public string IconSet => Self["iconSet"] ?? "mat-icons";

    public bool Disabled => Self.parseBoolAttr("disabled") ?? false;







    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnClick => Self.Handler != null ? KV($"OnHandlerCall", FN(Self.Handler)) : null;

    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("ApiOnAfterClick", "onafterclick")]
    public KeyValue OnAfterClick =>
      Self.AfterClick != null
      ?
      KV($"ApiOnAfterClick", FN(Self.App.jsrv.Action(Self.AfterClick).Call()))
      :
      null;
  }
}
