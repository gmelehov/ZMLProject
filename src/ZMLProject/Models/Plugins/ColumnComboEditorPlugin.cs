using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для столбца таблицы данных с инлайн-редактором-комбобоксом.
  /// </summary>
  public class ColumnComboEditorPlugin : BasePlugin
  {
    public ColumnComboEditorPlugin(Field field) : base(field)
    {

    }






    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType,
      KV_DataSrc,
      KV_EditorValueField
      );







    public Field Component => _component as Field;





    public override string PType => "ColumnComboEditor";
    public override string PluginId => null;




    public KeyValue KV_DataSrc => Component?.DataSrc != null ? KV("DataSrc", Component.DataSrc) : null;
    public KeyValue KV_EditorValueField => Component?.EditorValueField != null ? KV("editorValueField", Component.EditorValueField) : null;

  }
}
