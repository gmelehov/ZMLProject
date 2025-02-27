using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;






namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Поле ввода времени.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Value, ElProps.Seconds, ElProps.Minutes, ElProps.Hours)]
  [ManagedProps(PropAccess.Setter, ElProps.Value, ElProps.Seconds, ElProps.Minutes, ElProps.Hours)]
  [ManagedProps(PropAccess.Caller, "reset")]
  public class TimeField : BaseFormField
  {
    public TimeField(IZmlContainer host, XElement source) : base(host, source)
    {

    }









    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Format,
      KV_Increment
      );









    public override string XType => App.bsrv.TimeField(this).XType;

    public string Format => this["format"];

    public int? Increment => parseIntAttr("increment");









    public KeyValue KV_Format => PROP("format", Format);


    public KeyValue KV_Increment => PROP("increment", Increment);
  }
}
