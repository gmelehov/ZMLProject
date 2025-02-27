using DiffCode.JSGenerator;
using ZMLProject.Models.Forms;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class InputLocator : Locator<BaseFormField>
  {
    public InputLocator(string glob, string app, string id) : base(glob, app, id) { }








    public MemberAccess GetValue() => Self()?.AddExpr(CALL("getValue", Args()));
    public MemberAccess GetDay() => Self()?.AddExpr(CALL("GetDay", Args()));
    public MemberAccess GetMonth() => Self()?.AddExpr(CALL("GetMonth", Args()));
    public MemberAccess GetYear() => Self()?.AddExpr(CALL("GetYear", Args()));
    public MemberAccess GetHours() => Self()?.AddExpr(CALL("GetHours", Args()));
    public MemberAccess GetMinutes() => Self()?.AddExpr(CALL("GetMinutes", Args()));
    public MemberAccess GetSeconds() => Self()?.AddExpr(CALL("GetSeconds", Args()));
    public MemberAccess SetValue(AbstractExpression val) => Self()?.AddExpr(CALL("setValue", Args(val)));
    public MemberAccess SetDay(AbstractExpression val) => Self()?.AddExpr(CALL("SetDay", Args(val)));
    public MemberAccess SetMonth(AbstractExpression val) => Self()?.AddExpr(CALL("SetMonth", Args(val)));
    public MemberAccess SetYear(AbstractExpression val) => Self()?.AddExpr(CALL("SetYear", Args(val)));
    public MemberAccess SetHours(AbstractExpression val) => Self()?.AddExpr(CALL("SetHours", Args(val)));
    public MemberAccess SetMinutes(AbstractExpression val) => Self()?.AddExpr(CALL("SetMinutes", Args(val)));
    public MemberAccess SetSeconds(AbstractExpression val) => Self()?.AddExpr(CALL("SetSeconds", Args(val)));
    public MemberAccess SetFromValue(AbstractExpression val) => Self()?.AddExpr(CALL("setFromValue", Args(val)));
    public MemberAccess SetToValue(AbstractExpression val) => Self()?.AddExpr(CALL("setToValue", Args(val)));
    public MemberAccess GetStep() => Self()?.AddExpr(CALL("GetStep", Args()));
    public MemberAccess GetMinValue() => Self()?.AddExpr(CALL("GetMinValue", Args()));
    public MemberAccess GetMaxValue() => Self()?.AddExpr(CALL("GetMaxValue", Args()));
    public MemberAccess SetStep(AbstractExpression val) => Self()?.AddExpr(CALL("SetStep", Args(val)));
    public MemberAccess SetMinValue(AbstractExpression val) => Self()?.AddExpr(CALL("setMinValue", Args(val)));
    public MemberAccess SetMaxValue(AbstractExpression val) => Self()?.AddExpr(CALL("setMaxValue", Args(val)));
    public MemberAccess GetKeyPart() => Self()?.AddExpr(CALL("GetKeyPart", Args()));
    public MemberAccess GetValuePart() => Self()?.AddExpr(CALL("GetValuePart", Args()));
    public MemberAccess SetKeyPart(AbstractExpression val) => Self()?.AddExpr(CALL("SetKeyPart", Args(val)));
    public MemberAccess SetValuePart(AbstractExpression val) => Self()?.AddExpr(CALL("SetValuePart", Args(val)));
    public MemberAccess SetDelimiter(AbstractExpression val) => Self()?.AddExpr(CALL("SetDelimiter", Args(val)));
    public MemberAccess Reset() => Self()?.AddExpr(CALL("reset", Args()));









    public override string Manager => "Input";
  }
}
