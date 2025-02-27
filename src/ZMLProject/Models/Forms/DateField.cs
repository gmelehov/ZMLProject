using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;






namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Поле ввода даты.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Value, ElProps.Day, ElProps.Month, ElProps.Year)]
  [ManagedProps(PropAccess.Setter, ElProps.Value, ElProps.Day, ElProps.Month, ElProps.Year)]
  [ManagedProps(PropAccess.Caller, "reset")]
  public class DateField : BaseFormField
  {
    public DateField(IZmlContainer host, XElement source) : base(host, source)
    {

    }










    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Format,
      KV_StartDay
      );









    public override string XType => App.bsrv.DateField(this).XType;

    public string Format => this["format"];

    public string StartDay => this["startDay"];






    public KeyValue KV_Format => PROP("format", Format);

    public KeyValue KV_StartDay => PROP("startDay", StartDay);
  }
}
