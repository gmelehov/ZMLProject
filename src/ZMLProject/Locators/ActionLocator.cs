using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class ActionLocator : Locator<BaseAction>
  {
    public ActionLocator(string glob, string app, string id) : base(glob, app, id) { }







    public override MemberAccess Self() => new MemberAccess(ID(_global), ID(_app), ID(Manager)).AddExpr(CALL(Id, Args()));






    public MemberAccess Call() => Self();






    public override string Manager => "Action";
  }
}
