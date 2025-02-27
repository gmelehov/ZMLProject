using ZMLProject.Models.Forms;



namespace ZMLProject.Builders;

/// <summary>
/// 
/// </summary>
public class BaseComboBuilder : Builder<BaseCombo>
{
  public BaseComboBuilder(BaseCombo component) : base(component) { }






  public string FieldCls => Self.Editable == true && Self.ForceSelection == false ? "x-form-field basecombo no-trigger" : null;

  public int? MinChars => Self.Editable == true && Self.ForceSelection == false ? 2 as int? : null;

  public bool? HideTrigger => Self.Editable == true && Self.ForceSelection == false ? true as bool? : null;

  public string DefaultTemplateId => $"BaseComboList";

  public string DefaultTemplateItemSelector => $"combo-item";

}
