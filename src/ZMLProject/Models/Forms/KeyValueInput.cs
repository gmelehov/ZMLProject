using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;







namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Составной элемент ввода данных формата "КЛЮЧ: ЗНАЧЕНИЕ".
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Value, ElProps.KeyPart, ElProps.ValuePart)]
  [ManagedProps(PropAccess.Setter, ElProps.Delimiter, ElProps.KeyPart, ElProps.ValuePart)]
  public class KeyValueInput : BaseFormField
  {
    public KeyValueInput(IZmlContainer host, XElement source) : base(host, source)
    {
      
    }








    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Delimiter,
      KV_ValueType
      );








    /// <summary>
    /// 
    /// </summary>
    public override string XType => App.bsrv.KeyValueInput(this).XType;

    /// <summary>
    /// Разделитель ключа и значения, используемый для построения 
    /// составного текстового значения этого элемента ввода.
    /// </summary>
    public string Delimiter => this["delimiter"];

    /// <summary>
    /// Тип данных для поля ввода значения.
    /// </summary>
    public string ValueType => this["valueType"];








    public KeyValue KV_Delimiter => PROP("delimiter", Delimiter);

    public KeyValue KV_ValueType => PROP("valueType", ValueType);

  }
}
