using DiffCode.JSGenerator;
using ZMLProject.Models.Grids;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для включения инлайн-редактирования строк/ячеек в таблице данных/дереве узлов.
  /// </summary>
  public class GridEditingPlugin : BasePlugin
  {
    public GridEditingPlugin(Grid grid) : base(grid)
    {

    }
    public GridEditingPlugin(Tree tree) : base(tree)
    {

    }










    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType,
      KV_PluginId,
      KV_ClicksToEdit
      );





    public Grid GridComponent => _component as Grid;
    public Tree TreeComponent => _component as Tree;





    public override string PType => GridComponent?.Editor ?? TreeComponent?.Editor;
    public override string PluginId => GridComponent?.EditorId ?? TreeComponent?.EditorId;
    public int ClicksToEdit => 2;




    public KeyValue KV_ClicksToEdit => KV("clicksToEdit", ClicksToEdit);
  }
}
