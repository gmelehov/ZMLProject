using DiffCode.JSGenerator;
using ZMLProject.Models.Forms;


namespace ZMLProject.Builders;

/// <summary>
/// 
/// </summary>
public class BaseFormFieldBuilder : Builder<BaseFormField>
{
  public BaseFormFieldBuilder(BaseFormField component) : base(component) { }






  /// <summary>
  /// 
  /// </summary>
  [EventDescriptor("ApiOnChange", "onchange")]
  public KeyValue ApiOnChange => Self.OnEvent($"ApiOnChange", $"onchange");
}
