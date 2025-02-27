using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class PanelLocator : Locator<Panel>
  {
    public PanelLocator(string glob, string app, string id) : base(glob, app, id) { }








    public MemberAccess GetCollapsed() => Self()?.AddExpr(CALL("GetCollapsed", Args()));
    public MemberAccess SetCollapsed(AbstractExpression val) => Self()?.AddExpr(CALL("SetCollapsed", Args(val)));
    public MemberAccess AddCls(AbstractExpression val) => Self()?.AddExpr(CALL("addCls", Args(val)));
    public MemberAccess RemoveCls(AbstractExpression val) => Self()?.AddExpr(CALL("removeCls", Args(val)));
    public MemberAccess AddBodyCls(AbstractExpression val) => Self()?.AddExpr(CALL("addBodyCls", Args(val)));
    public MemberAccess RemoveBodyCls(AbstractExpression val) => Self()?.AddExpr(CALL("removeBodyCls", Args(val)));
    public MemberAccess AddHtmlContent(AbstractExpression val) => Self()?.AddExpr(CALL("AddHtmlContent", Args(val)));
    public MemberAccess RemoveHtmlContent() => Self()?.AddExpr(CALL("RemoveHtmlContent", Args()));
    public MemberAccess GetLoader() => Self()?.AddExpr(CALL("GetLoaderId", Args()));
    public MemberAccess SetLoader(AbstractExpression val) => Self()?.AddExpr(CALL("SetLoader", Args(val)));







    public override string Manager => "Panel";
  }
}
