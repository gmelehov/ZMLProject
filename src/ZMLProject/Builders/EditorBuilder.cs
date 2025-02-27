using DiffCode.JSGenerator;






namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class EditorBuilder : Builder<Editor>
  {
    public EditorBuilder(Editor component) : base(component) { }








    public string XType => "CodeMirrorPanel";
    public string Layout => Self["layout"] ?? "fit";






    /// <summary>
    /// Событие onchange.
    /// </summary>
    [EventDescriptor("OnAfterChanged", "onchange")]
    public KeyValue OnAfterChanged => Self.OnEvent("OnAfterChanged", "onchange");
  }
}
