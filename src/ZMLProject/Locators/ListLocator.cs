using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class ListLocator : Locator<List>
  {
    public ListLocator(string glob, string app, string id) : base(glob, app, id) { }







    public MemberAccess GetRecord(string fieldName, int index = 0) => Self()?.AddExpr(CALL("GetSelectedRowField", Args(Str(fieldName), Num(index))));
    public MemberAccess CountTotal() => Self()?.AddExpr(CALL("CountTotal", Args()));
    public MemberAccess CountSelected() => Self()?.AddExpr(CALL("CountSelected", Args()));








    public override string Manager => "List";
  }
}
