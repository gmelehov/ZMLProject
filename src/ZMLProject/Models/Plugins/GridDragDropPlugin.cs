using DiffCode.JSGenerator;
using ZMLProject.Models.Grids;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для перетаскивания и переупорядочивания строк таблицы данных.
  /// </summary>
  public class GridDragDropPlugin : BasePlugin
  {
    public GridDragDropPlugin(Grid grid) : base(grid)
    {

    }







    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType,
      KV_ContainerScroll
      );





    public override string PType => "gridviewdragdrop";
    public override string PluginId => null;
    public bool ContainerScroll => true;






    public KeyValue KV_ContainerScroll => KV("containerScroll", ContainerScroll);

  }
}
