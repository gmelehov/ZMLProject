using DiffCode.JSGenerator;


namespace ZMLProject.Builders;

/// <summary>
/// 
/// </summary>
public class BaseSourceBuilder : Builder<BaseSource>
{
  public BaseSourceBuilder(BaseSource component) : base(component) { }






  public string Classname => $"ZML.source.{Self.Id}";

  public int PageSize => (Self["pageSize"] == null || Self["pageSize"] == "auto") ? 1 : Self.parseIntAttr("pageSize") ?? 1;

  public bool AutoLoad => Self.parseBoolAttr("autoload") ?? false;

  public bool AutoSync => Self.parseBoolAttr("autosync") ?? true;







  /// <summary>
  /// 
  /// </summary>
  [EventDescriptor("OnAfterLoad", "onload")]
  public KeyValue OnAfterLoad => Self.OnEvent($"OnAfterLoad", $"onload");

  /// <summary>
  /// 
  /// </summary>
  [EventDescriptor("OnAfterWrite", "onwrite")]
  public KeyValue OnAfterWrite => Self.OnEvent($"OnAfterWrite", $"onwrite");
}
