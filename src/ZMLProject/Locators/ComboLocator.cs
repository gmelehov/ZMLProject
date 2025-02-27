using DiffCode.JSGenerator;
using ZMLProject.Models.Forms;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class ComboLocator : Locator<BaseCombo>
  {
    public ComboLocator(string glob, string app, string id) : base(glob, app, id) { }









    public MemberAccess Reset() => Self()?.AddExpr(CALL("reset", Args()));
    public MemberAccess GetValue() => Self()?.AddExpr(CALL("GetSelectedValue", Args()));
    public MemberAccess GetDisplayValue() => Self()?.AddExpr(CALL("getDisplayValue", Args()));
    public MemberAccess GetDescriptionValue() => Self()?.AddExpr(CALL("getDescriptionValue", Args()));
    public MemberAccess CountTotal() => Self()?.AddExpr(CALL("GetTotalCount", Args()));
    public MemberAccess SetValue(AbstractExpression val) => Self()?.AddExpr(CALL("setValue", Args(val)));






    public override string Manager => "Combo";
  }
}
