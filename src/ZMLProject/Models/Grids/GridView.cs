using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Plugins;






namespace ZMLProject.Models.Grids
{
  /// <summary>
  /// Вспомогательный класс: внутреннее представление таблицы данных (GridView).
  /// </summary>
  public class GridView
  {
    private Grid _grid;
    private GridDragDropPlugin _ddplugin;





    public GridView(Grid grid)
    {
      _grid = grid;
      if (_grid.RowsDragDrop)
      {
        _ddplugin = new GridDragDropPlugin(_grid);
      };
    }








    public ObjectBody AsConfig => OBJ(false, false,
      KV_StripeRows,
      KV_TextSelection,
      KV_Plugins
      );










    public KeyValue KV_StripeRows => _grid?.StripeRows != null ? KV("stripeRows", _grid.StripeRows) : null;

    public KeyValue KV_TextSelection => _grid?.TextSelection != null ? KV("enableTextSelection", _grid.TextSelection) : null;

    public KeyValue KV_Plugins => _ddplugin != null ? KV("plugins", Arr(false, false, _ddplugin?.AsConfig)) : null;

  }
}
