using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;






namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class LoaderLocator : Locator<Loader>
  {
    public LoaderLocator(string glob, string app, string id) : base(glob, app, id) { }








    public MemberAccess MGR_Register(AbstractExpression val) => MGR().AddExpr(CALL("register", Args(val)));

    public MemberAccess GetMode() => Self()?.AddExpr(CALL("GetRenderMode", Args()));
    public MemberAccess GetUrl() => Self()?.AddExpr(CALL("GetUrl", Args()));
    public MemberAccess SetUrl(AbstractExpression val) => Self()?.AddExpr(CALL("SetUrl", Args(val)));
    public MemberAccess Load() => Self()?.AddExpr(CALL("Load", Args()));







    public override string Manager => "Loader";
  }
}
