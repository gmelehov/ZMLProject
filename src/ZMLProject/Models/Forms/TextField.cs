using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;







namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Поле для ввода текста.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Value)]
  [ManagedProps(PropAccess.Setter, ElProps.Value)]
  [ManagedProps(PropAccess.Caller, "reset")]
  public class TextField : BaseFormField
  {
    public TextField(IZmlContainer host, XElement source) : base(host, source)
    {

    }








    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_ConverQuotes,
      KV_EmptyText
      );










    public override string XType => App.bsrv.TextField(this).XType;

    public bool? ConvertQuotes => parseBoolAttr("convertQuotes");

    public string EmptyText => this["emptyText"];









    public KeyValue KV_ConverQuotes => PROP("convertQuotes", ConvertQuotes);


    public KeyValue KV_EmptyText => PROP("emptyText", EmptyText);
  }
}
