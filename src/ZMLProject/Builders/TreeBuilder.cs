namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class TreeBuilder : Builder<Tree>
  {
    public TreeBuilder(Tree component) : base(component) { }






    public string XType => "CommonTree";

    public bool Expanded => Self.parseBoolAttr("expanded") ?? false;

    public string Sel => Self["sel"] ?? "single";

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

    public string Editor => "cellediting";

    public string EditorId => "cedit";

  }
}
