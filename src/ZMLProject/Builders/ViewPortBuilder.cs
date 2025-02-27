namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class ViewPortBuilder : Builder<ViewPort>
  {
    public ViewPortBuilder(ViewPort component) : base(component) { }










    public string Id => "AppViewPort";

    public string Classname => $"ZML.view.App";

    public string Extend => Self.Target != null ? $"Ext.panel.Panel" : $"Ext.container.Viewport";

    public string Alias => $"widget.{Id}";

    public string Layout
    {
      get
      {
        var ret = Self["layout"];

        if (ret == null) ret = "fit";
        if (ret == "multiaccordion") ret = "accordion";

        return ret;
      }
    }

  }
}
