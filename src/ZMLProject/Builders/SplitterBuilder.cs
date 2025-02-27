namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class SplitterBuilder : Builder<Splitter>
  {
    public SplitterBuilder(Splitter component) : base(component) { }






    public string XType => "splitter";

    public string OverCls => Self != null ? Self["overCls"] ?? OverCls_Default : OverCls_Default;

    public string CollapseTarget => Self != null ? Self["collapseTarget"] ?? CollapseTarget_Default : CollapseTarget_Default;




    public string OverCls_Default => "x-splitter-over";

    public string CollapseTarget_Default => "next";

  }
}
