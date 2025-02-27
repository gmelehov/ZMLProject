using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;







namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Поле ввода чисел.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Value, ElProps.MinValue, ElProps.MaxValue, ElProps.Step)]
  [ManagedProps(PropAccess.Setter, ElProps.Value, ElProps.MinValue, ElProps.MaxValue, ElProps.Step)]
  [ManagedProps(PropAccess.Caller, "reset")]
  public class NumberField : BaseFormField
  {
    public NumberField(IZmlContainer host, XElement source) : base(host, source)
    {

    }









    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_DecimalPrecision,
      KV_MinValue,
      KV_MaxValue,
      KV_Step,
      KV_Value
      );









    /// <summary>
    /// Краткий псевдоним ExtJS-класса для этого компонента.
    /// </summary>
    public override string XType => App.bsrv.NumberField(this).XType;

    /// <summary>
    /// Количество знаков после запятой, отображаемое в этом поле ввода.
    /// </summary>
    public int? DecimalPrecision => parseIntAttr("decimalPrecision");

    /// <summary>
    /// Минимальное значение для этого поля ввода.
    /// </summary>
    public double? MinValue => parseDoubleAttr("minValue");

    /// <summary>
    /// Максимальное значение для этого поля ввода.
    /// </summary>
    public double? MaxValue => parseDoubleAttr("maxValue");

    /// <summary>
    /// Шаг изменения значения в этом поле ввода при использовании встроенного спиннера.
    /// </summary>
    public double? Step => parseDoubleAttr("step");

    /// <summary>
    /// Начальное значение для этого поля ввода.
    /// </summary>
    public virtual double? Value => parseDoubleAttr("value");









    public KeyValue KV_DecimalPrecision => PROP("decimalPrecision", DecimalPrecision);


    public KeyValue KV_MinValue => PROP("minValue", MinValue);


    public KeyValue KV_MaxValue => PROP("maxValue", MaxValue);


    public KeyValue KV_Step => PROP("step", Step);


    public virtual KeyValue KV_Value => PROP("value", Value);
  }
}
