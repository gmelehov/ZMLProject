using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;




namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для кнопки, добавляющий к ней эффект Ripple при нажатии.
  /// </summary>
  public class RipplePlugin : BasePlugin
  {
    public RipplePlugin(IHasContextBinders component) : base(component)
    {

    }






    public override ObjectBody AsConfig => OBJ(false, false,
      KV_PType,
      KV_PluginId
      );





    public IHasContextBinders Component => _component as IHasContextBinders;




    public override string PType => "Ripple";
    public override string PluginId => $"{(Component != null ? $"{Component?.Id}-" : "")}ripplePlugin";

  }
}
