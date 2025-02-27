using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;




namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин, управляющий цветами заливки и текста для заголовка и содержимого
  /// всех элементов, наследующих функциональность абстрактной панели.
  /// </summary>
  public class ColorManagerPlugin : BasePlugin
  {
    public ColorManagerPlugin(BasePanel panel) : base(panel)
    {

    }







    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType,
      KV_TitleColor,
      KV_TitleBackColor,
      KV_BodyBackColor
      );





    public BasePanel Component => _component as BasePanel;





    public override string PType => "ColorManager";
    public override string PluginId => null;





    public KeyValue KV_TitleColor => KV("titleColor", Component?.TitleColor);
    public KeyValue KV_TitleBackColor => KV("titleBackColor", Component?.TitleBackColor);
    public KeyValue KV_BodyBackColor => KV("bodyBackColor", Component?.BodyBackColor);

  }
}
