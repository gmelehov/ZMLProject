using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;




namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Плагин для управления состоянием элемента 
  /// при помощи событий из внешнего контекста.
  /// Плагин подключается к целевому элементу автоматически,
  /// в случае если в элементе присутствуют дочерние элементы типа <see cref="ContextBinder"/>
  /// </summary>
  public class HostContextPlugin : BasePlugin
  {
    public HostContextPlugin(IHasContextBinders component) : base(component)
    {

    }






    public override ObjectBody AsConfig => OBJ(false, true,
      KV_PType,
      KV_PluginId,
      KV_ContextBinders
      );






    public IHasContextBinders Component => _component as IHasContextBinders;








    public override string PType => Component?.As<ZmlContainer>()?.App.bsrv.HostContextPlugin(this).PType;

    public override string PluginId => Component?.As<ZmlContainer>()?.App.bsrv.HostContextPlugin(this).PluginId;









    public KeyValue KV_ContextBinders => KV("CTX_BINDERS", Arr(false, true, Component?.ContextBinders?.Select(s => s.AsConfig)));
  }
}
