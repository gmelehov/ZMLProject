using DiffCode.JSGenerator;
using ZMLProject.Models.Grids;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class GridLocator : Locator<Grid>
  {
    public GridLocator(string glob, string app, string id) : base(glob, app, id) { }







    public MemberAccess GetSelectedCount() => Self()?.AddExpr(CALL("GetSelectedCount", Args()));
    public MemberAccess GetSelectedKey() => Self()?.AddExpr(CALL("GetSelectedKeys", Args(TRUE)))[0];
    public MemberAccess GetKey() => Self()?.AddExpr(CALL("GetKeyName", Args()));
    public MemberAccess GetRecord(string fieldName, int index = 0) => Self()?.AddExpr(CALL("GetSelectedRowField", Args(Str(fieldName), Num(index))));
    public MemberAccess GetSrc() => Self()?.AddExpr(CALL("GetSourceId", Args()));
    public MemberAccess SetRecord(string fieldName, AbstractExpression val, int index = 0) => Self()?.AddExpr(CALL("SetSelectedRowField", Args(Str(fieldName), val, Num(index))));






    public override string Manager => "Grid";
  }
}
