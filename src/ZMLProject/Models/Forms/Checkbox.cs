using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Enums;



namespace ZMLProject.Models.Forms;

/// <summary>
/// Чекбокс.
/// </summary>
[ManagedProps(PropAccess.Getter, ElProps.Value)]
[ManagedProps(PropAccess.Setter, ElProps.Value)]
[ManagedProps(PropAccess.Caller, "reset")]
public class Checkbox : BaseFormField
{
  public Checkbox(IZmlContainer host, XElement source) : base(host, source)
  {

  }






  protected override ObjectBody SpecificKeyValues => OBJ(
    KV_Checked,
    KV_InputValue
    );








  public override string XType => App.bsrv.Checkbox(this).XType;

  public bool? Checked => parseBoolAttr("checked");

  public string InputValue => this["inputValue"];








  public KeyValue KV_Checked => PROP("checked", Checked);
  
  public KeyValue KV_InputValue => PROP("inputValue", InputValue);

}
