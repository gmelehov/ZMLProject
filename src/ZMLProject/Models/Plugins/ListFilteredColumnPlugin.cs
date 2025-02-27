using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для столбца таблицы данных с инлайн-редактором-комбобоксом.
  /// </summary>
  public class ListFilteredColumnPlugin : BasePlugin
  {
    public ListFilteredColumnPlugin(Field field) : base(field)
    {

    }






    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType
      );





    public Field Component => _component as Field;





    public override string PType => "ListFilteredColumn";
    public override string PluginId => null;

  }
}
