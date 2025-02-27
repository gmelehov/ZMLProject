using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;







namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Поле ввода диапазона чисел.
  /// </summary>
  [ManagedProps(PropAccess.Setter, ElProps.FromValue, ElProps.ToValue)]
  public class NumberSet : NumberField
  {
    public NumberSet(IZmlContainer host, XElement source) : base(host, source)
    {

    }








    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_DecimalPrecision,
      KV_Delimiter,
      KV_MinValue,
      KV_MaxValue,
      KV_Step
      );








    public override string XType => App.bsrv.NumberSet(this).XType;

    public string Delimiter => this["delimiter"];

    public override double? Value => null;








    public KeyValue KV_Delimiter => PROP("delimiter", Delimiter);
  }
}
