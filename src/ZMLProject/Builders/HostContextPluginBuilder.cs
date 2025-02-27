using ZMLProject.Models.Plugins;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class HostContextPluginBuilder : Builder<HostContextPlugin>
  {
    public HostContextPluginBuilder(HostContextPlugin component) : base(component) { }









    public string PType => "HostContext";

    public string PluginId => $"{(Self.Component != null ? $"{Self.Component?.Id}-" : "")}hostContextPlugin";

  }
}
