using DiffCode.CommonExtensions;
using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Models.Grids
{
  /// <summary>
  /// Вспомогательный класс: пейджинг-тулбар таблицы данных.
  /// </summary>
  public class GridPaging : ZmlComponent
  {
    private Grid _grid;





    public GridPaging(Grid grid) : base(null)
    {
      _grid = grid;
    }







    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, false,
      KV_XType,
      KV_StoreId,
      KV_PageSize,
      KV_CrudButtons,
      KV_ClipboardButtons,
      KV_EnableCreateButton,
      Grid?.App.bsrv.GridPaging(this).OnCreateClicked,
      Grid?.App.bsrv.GridPaging(this).OnDeleteClicked,
      Grid?.App.bsrv.GridPaging(this).OnUpdateClicked
      );










    public string XType => Grid?.App.bsrv.GridPaging(this).XType;

    public bool CrudButtons => _grid.IsEditable || _grid.Create.IsUseful() || _grid.Update.IsUseful() || _grid.Delete.IsUseful();

    public bool ClipBoardbuttons => _grid.Clipboard;







    public Grid Grid => _grid;








    public KeyValue KV_XType => KV("xtype", XType);

    public KeyValue KV_StoreId => KV("store", _grid.Src);
    
    public KeyValue KV_PageSize => KV("defaultpagesize", _grid.Source.PageSize);

    public KeyValue KV_CrudButtons => KV("crudButtons", CrudButtons);

    public KeyValue KV_ClipboardButtons => KV("clipboardButtons", ClipBoardbuttons);

    public KeyValue KV_EnableCreateButton => _grid.Create.IsUseful() ? KV("enableCreateButton", TRUE) : null;

  }
}
