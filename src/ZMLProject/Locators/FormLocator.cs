using DiffCode.JSGenerator;
using ZMLProject.Models.Forms;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class FormLocator : Locator<Form>
  {
    public FormLocator(string glob, string app, string id) : base(glob, app, id) { }







    public MemberAccess AddCls(AbstractExpression val) => Self()?.AddExpr(CALL("addCls", Args(val)));
    public MemberAccess RemoveCls(AbstractExpression val) => Self()?.AddExpr(CALL("removeCls", Args(val)));
    public MemberAccess AddBodyCls(AbstractExpression val) => Self()?.AddExpr(CALL("addBodyCls", Args(val)));
    public MemberAccess RemoveBodyCls(AbstractExpression val) => Self()?.AddExpr(CALL("removeBodyCls", Args(val)));








    public override string Manager => "Form";
  }
}
