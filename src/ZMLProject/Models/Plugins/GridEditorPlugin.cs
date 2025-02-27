using DiffCode.JSGenerator;
using ZMLProject.Models.Grids;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для подключения к тулбару таблицы данных кнопок с CRUD-функционалом.
  /// </summary>
  public class GridEditorPlugin : BasePlugin
  {
    public GridEditorPlugin(Grid grid) : base(grid)
    {

    }








    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType
      );






    public Grid Component => _component as Grid;










    public override string PType => "GridEditor";
    
    
    public override string PluginId => null;

  }
}
