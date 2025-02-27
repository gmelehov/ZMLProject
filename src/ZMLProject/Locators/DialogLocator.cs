using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class DialogLocator : Locator<Dialog>
  {
    public DialogLocator(string glob, string app, string id) : base(glob, app, id) { }







    public MemberAccess Show() => Self()?.AddExpr(CALL("show", Args()));
    public MemberAccess AddCls(AbstractExpression val) => Self()?.AddExpr(CALL("addCls", Args(val)));
    public MemberAccess RemoveCls(AbstractExpression val) => Self()?.AddExpr(CALL("removeCls", Args(val)));
    public MemberAccess AddBodyCls(AbstractExpression val) => Self()?.AddExpr(CALL("addBodyCls", Args(val)));
    public MemberAccess RemoveBodyCls(AbstractExpression val) => Self()?.AddExpr(CALL("removeBodyCls", Args(val)));







    public override string Manager => "Dialog";
  }
}
