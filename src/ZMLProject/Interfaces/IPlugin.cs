using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Плагин.
/// </summary>
public interface IPlugin
{

  /// <summary>
  /// 
  /// </summary>
  ObjectBody AsConfig { get; }

  /// <summary>
  /// 
  /// </summary>
  string PType { get; }

  /// <summary>
  /// 
  /// </summary>
  string PluginId { get; }








  KeyValue KV_PType { get; }


  KeyValue KV_PluginId { get; }
}
