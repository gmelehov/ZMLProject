using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class SourceLocator : Locator<BaseSource>
  {
    public SourceLocator(string glob, string app, string id) : base(glob, app, id) { }








    public MemberAccess GetTotalCount() => Self()?.AddExpr(CALL("getCount", Args()));
    public MemberAccess GetPageSize() => Self()?.AddExpr(ID("pageSize"));
    public MemberAccess GetTotalPages() => Self()?.AddExpr(CALL("GetTotalPages", Args()));
    public MemberAccess GetCurrentPage() => Self()?.AddExpr(ID("currentPage"));
    public MemberAccess GetRecord(string fieldName, int index = 0) => Self()?.AddExpr(CALL("GetRecordField", Args(Str(fieldName), Num(index))));
    public MemberAccess Load() => Self()?.AddExpr(CALL("load", Args()));







    public override string Manager => "Source";
  }
}
