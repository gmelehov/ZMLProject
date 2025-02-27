using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;






namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class BaseActionBuilder : Builder<BaseAction>
  {
    public BaseActionBuilder(BaseAction component) : base(component) { }






    public string Classname => $"ZML.data.Action";






    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnConfirmAfterResponse", "onaccept")]
    public KeyValue OnAccepted => Self.OnEvent($"OnConfirmAfterResponse", $"onaccept");

    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnCancelAfterResponse", "onreject")]
    public KeyValue OnRejected => Self.OnEvent($"OnCancelAfterResponse", $"onreject");

    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnAfterLoad =>
      Self["onload"] != null ?
      Self.OnEvent($"OnAfterLoad", $"onload")
      :
      Self.Commands.Count() > 0 ?
      KV($"OnAfterLoad", FN(Self.Commands))
      :
      null;
  }
}
