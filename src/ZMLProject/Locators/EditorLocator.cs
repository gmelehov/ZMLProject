using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class EditorLocator : Locator<Editor>
  {
    public EditorLocator(string glob, string app, string id) : base(glob, app, id) { }








    public MemberAccess GetValue() => Self()?.AddExpr(CALL("getCodeValue", Args()));
    public MemberAccess GetHtmlContent() => Self()?.AddExpr(CALL("GetRenderedMarkdown", Args()));
    public MemberAccess GetMode() => Self()?.AddExpr(CALL("getMode", Args()));
    public MemberAccess SetValue(AbstractExpression val) => Self()?.AddExpr(CALL("setValue", Args(val)));
    public MemberAccess SetMode(AbstractExpression val) => Self()?.AddExpr(CALL("setMode", Args(val)));
    public MemberAccess Clear() => Self()?.AddExpr(CALL("Clear", Args()));








    public override string Manager => "Editor";
  }
}
