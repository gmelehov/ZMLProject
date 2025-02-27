using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;








namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Многострочное поле для ввода текста.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Value)]
  [ManagedProps(PropAccess.Setter, ElProps.Value)]
  [ManagedProps(PropAccess.Caller, "reset")]
  public class TextArea : TextField
  {
    public TextArea(IZmlContainer host, XElement source) : base(host, source)
    {

    }








    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_ConverQuotes,
      KV_EmptyText,
      KV_Rows
      );








    public override string XType => App.bsrv.TextArea(this).XType;

    public int? Rows => parseIntAttr("rows");








    public KeyValue KV_Rows => PROP("rows", Rows);
  }
}
