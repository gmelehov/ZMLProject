namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class TabpanelBuilder : Builder<Tabpanel>
  {
    public TabpanelBuilder(Tabpanel component) : base(component) { }






    public string XType => "Tabpanel";

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
