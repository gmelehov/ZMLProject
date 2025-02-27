using DiffCode.CommonExtensions;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Grids;






namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class GridPagingBuilder : Builder<GridPaging>
  {
    public GridPagingBuilder(GridPaging component) : base(component) { }






    public string XType => "Paging";







    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnCreateClicked => Self.Grid["create"].IsUseful() ? KV("OnCreateClicked", FN(Self.Grid.App.jsrv.Action(Self.Grid["create"]).Call())) : null;

    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnDeleteClicked => Self.Grid["delete"].IsUseful() ? KV("OnDeleteClicked", FN(Self.Grid.App.jsrv.Action(Self.Grid["delete"]).Call())) : null;

    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnUpdateClicked => Self.Grid["update"].IsUseful() ? KV("OnUpdateClicked", FN(Self.Grid.App.jsrv.Action(Self.Grid["update"]).Call())) : null;
  }
}
