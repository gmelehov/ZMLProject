using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class TreeLocator : Locator<Tree>
  {
    public TreeLocator(string glob, string app, string id) : base(glob, app, id) { }








    public MemberAccess GetRecord(string fieldName, int index = 0) => Self()?.AddExpr(CALL("GetSelectedRowField", Args(Str(fieldName), Num(index))));







    public override string Manager => "Tree";
  }
}
