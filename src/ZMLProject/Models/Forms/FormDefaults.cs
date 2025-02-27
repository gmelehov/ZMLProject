using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;






namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Общие настройки для полей формы ввода.
  /// </summary>
  public class FormDefaults : BaseFormItem, IFormDefaults
  {
    public FormDefaults(Form host, XElement source) : base(host, source)
    {

    }








    public override ObjectBody AsConfig => OBJ(false, false,
      KV_Anchor,
      KV_ColumnWidth,
      KV_Flex,
      KV_Cls,
      KV_Width,
      KV_Height,
      KV_Margin,
      KV_Padding,
      KV_MinHeight,
      KV_MaxHeight,
      KV_MinWidth,
      KV_MaxWidth,
      KV_Disabled,
      KV_LabelAlign,
      KV_LabelWidth,
      KV_AllowBlank,
      KV_LabelClsExtra
      );



  }
}
