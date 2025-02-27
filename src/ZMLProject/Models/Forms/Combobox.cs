using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;






namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Комбобокс.
  /// </summary>
  public class Combobox : BaseCombo
  {
    public Combobox(IZmlContainer host, XElement source) : base(host, source)
    {
      
    }











    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Source,
      KV_ValueField,
      KV_DisplayField,
      KV_DescrField,
      KV_Editable,
      KV_MultiSelect,
      KV_ForceSelection,
      KV_HideTrigger,
      KV_DefaultValue,
      KV_FieldCls,
      KV_MinChars,
      KV_Template
      );











    /// <summary>
    /// 
    /// </summary>
    public override string XType => App.bsrv.Combobox(this).XType;

    /// <summary>
    /// 
    /// </summary>
    public string DescrField => this["descr"];

    /// <summary>
    /// 
    /// </summary>
    public string Store => this["src"];










    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_DescrField => PROP("descrField", DescrField);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Source => PROP("store", Store);
  }
}
