using DiffCode.JSGenerator;
using ZMLProject.Models.Grids;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class GridBuilder : Builder<Grid>
  {
    public GridBuilder(Grid component) : base(component) { }






    public string XType => "Grid";
    
    public bool Clipboard => Self.parseBoolAttr("clipboard") ?? false;
    
    public bool StripeRows => Self.parseBoolAttr("stripeRows") ?? true;
    
    public bool TextSelection => Self.parseBoolAttr("textSelection") ?? true;
    
    public bool RowsDragDrop => Self.parseBoolAttr("rowsDragDrop") ?? false;
    
    public string Sel => Self["sel"] ?? "single";
    
    public string Editor
    {
      get
      {
        var ret = Self["editor"] ?? "row";
        if (ret == "cell") ret = "cellediting";
        if (ret == "row") ret = "rowediting";
        return ret;
      }
    }
    
    public string EditorId => "cedit";
    
    public string SelMode
    {
      get
      {
        switch (Sel)
        {
          case "cell":
          case "single":
          case "single-check": return "SINGLE";
          case "multi": return "MULTI";

          default: return null;
        }
      }
    }
    
    public string SelectionModel => Sel == "cell" ? "cellmodel" : Sel == "single" ? "rowmodel" : "checkboxmodel";









    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnRowSelected", "onselect")]
    public KeyValue OnRowSelected => Self.OnEvent($"OnRowSelected", $"onselect");
    
    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnRowDeselected", "ondeselect")]
    public KeyValue OnRowDeselected => Self.OnEvent($"OnRowDeselected", $"ondeselect");
    
    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnRowDblClicked", "ondblclick")]
    public KeyValue OnRowDblClicked => Self.OnEvent($"OnRowDblClicked", $"ondblclick");
    
    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnDataLoaded", "ondataload")]
    public KeyValue OnDataLoaded => Self.OnEvent($"OnDataLoaded", $"ondataload");
    
    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnCtxMenu", "onctxmenu")]
    public KeyValue OnCtxMenu => Self.OnEvent($"OnCtxMenu", $"onctxmenu");
  }
}
