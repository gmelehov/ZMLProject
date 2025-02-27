using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;






namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Поле ввода диапазона дат.
  /// </summary>
  [ManagedProps(PropAccess.Setter, ElProps.FromValue, ElProps.ToValue)]
  public class DateSet : DateField
  {
    public DateSet(IZmlContainer host, XElement source) : base(host, source)
    {

    }









    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Format,
      KV_StartDay,
      KV_Delimiter
      );








    public override string XType => App.bsrv.DateSet(this).XType;

    public string Delimiter => this["delimiter"];







    public KeyValue KV_Delimiter => PROP("delimiter", Delimiter);
  }
}
