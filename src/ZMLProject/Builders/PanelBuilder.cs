namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class PanelBuilder : Builder<Panel>
  {
    public PanelBuilder(Panel component) : base(component) { }






    public string XType => "Panel";

    public string Layout
    {
      get
      {
        var ret = Self["layout"];
        if (ret == "multiaccordion") ret = "accordion";

        return ret;
      }
    }
  }
}
