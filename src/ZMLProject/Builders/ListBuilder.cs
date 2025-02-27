namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class ListBuilder : Builder<List>
  {
    public ListBuilder(List component) : base(component) { }







    public string XType => "List";
    
    public bool? HidePaging => Self.parseBoolAttr("hidePaging") ?? true;
    
    public bool? HideHeaders => Self.parseBoolAttr("hideHeaders") ?? true;
    
    public string Sel => Self["sel"] ?? "single";
    
    public string PagingDock => Self["pagingDock"] ?? "bottom";
    
    public string ItemSelector => Self.Template != null ? $".{Self.Template.ItemSelector}" : null;
    
    public string Layout => Self["layout"] ?? "fit";

  }
}
