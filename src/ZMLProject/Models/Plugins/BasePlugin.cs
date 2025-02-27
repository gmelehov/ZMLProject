using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Diagnostics;





namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Абстрактный плагин.
  /// </summary>
  public abstract class BasePlugin : ZmlComponent, IPlugin
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected IHasPlugins _component;





    protected BasePlugin(IHasPlugins component) : base(null) => _component = component;






    public override ExtJSClassExpression AsDefine => null;







    public abstract string PType { get; }
    
    public abstract string PluginId { get; }









    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_PType => PType != null ? KV("ptype", PType) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_PluginId => PluginId != null ? KV("pluginId", PluginId) : null;

  }
}
