using DiffCode.JSGenerator;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class DialogBuilder : Builder<Dialog>
  {
    public DialogBuilder(Dialog component) : base(component) { }







    public string XType => "Dialog";

    public string Layout
    {
      get
      {
        var ret = Self["layout"];
        if (ret == "multiaccordion") ret = "accordion";

        return ret;
      }
    }

    public bool StdButtons => Self.parseBoolAttr("stdButtons") ?? true;

    public bool Modal => Self.parseBoolAttr("modal") ?? true;







    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnDialogOpened", "onshow")]
    public KeyValue OnShow => Self.OnEvent($"OnDialogOpened", $"onshow");

    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnDialogCancelled", "oncancel")]
    public KeyValue OnCancel => Self.OnEvent($"OnDialogCancelled", $"oncancel");

    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnAfterConfirmed", "onafterconfirm")]
    public KeyValue OnAfterConfirm => Self.OnEvent($"OnAfterConfirmed", $"onafterconfirm");

  }
}
