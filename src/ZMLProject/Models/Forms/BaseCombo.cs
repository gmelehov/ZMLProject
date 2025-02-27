using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;
using ZMLProject.Enums;


namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Базовый класс для полей ввода данных, основанных на комбобоксе.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.DescrValue, ElProps.DisplayValue, ElProps.TotalCount, ElProps.Value)]
  [ManagedProps(PropAccess.Setter, ElProps.Value)]
  [ManagedProps(PropAccess.Caller, "reset")]
  public abstract class BaseCombo : BaseFormField
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected readonly Template _template;







    protected BaseCombo(IZmlContainer host, XElement source) : base(host, source)
    {
      var templSource = source.Element(F("Template"));
      if (templSource != null)
      {
        _template = new Template(this, templSource);
      }
      else
      {
        _template = GetDefaultTemplate();
      };
    }









    protected virtual Template GetDefaultTemplate()
    {
      var xelem = new XElement(F("Template"), new XAttribute("id", DefaultTemplateId), new XAttribute("itemSelector", DefaultTemplateItemSelector), new XAttribute("field_a", this.DisplayField));
      var templ = new Template(this, xelem);
      return templ;
    }










    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_ValueField,
      KV_DisplayField,
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
    public string DefaultTemplateId => App.bsrv.BaseCombo(this).DefaultTemplateId;

    /// <summary>
    /// 
    /// </summary>
    public string DefaultTemplateItemSelector => App.bsrv.BaseCombo(this).DefaultTemplateItemSelector;

    public virtual string DisplayField => this["display"];

    public virtual string ValueField => this["value"];

    public virtual bool MultiSelect => parseBoolAttr("multiSelect") ?? false;

    public virtual bool Editable => parseBoolAttr("editable") ?? false;

    public virtual bool ForceSelection => parseBoolAttr("forceSelection") ?? true;

    public virtual bool? HideTrigger => App.bsrv.BaseCombo(this).HideTrigger;

    public virtual string DefaultValue => this["defaultValue"];

    public virtual string FieldCls => App.bsrv.BaseCombo(this).FieldCls;

    public virtual int? MinChars => App.bsrv.BaseCombo(this).MinChars;











    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_ValueField => KV("valueField", ValueField);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_DisplayField => PROP("displayField", DisplayField);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Editable => PROP("editable", Editable);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_MultiSelect => PROP("multiSelect", MultiSelect);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_ForceSelection => PROP("forceSelection", ForceSelection);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_HideTrigger => PROP("hideTrigger", HideTrigger);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_DefaultValue => PROP("defaultValue", DefaultValue);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_FieldCls => PROP("fieldCls", FieldCls);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_MinChars => PROP("minChars", MinChars);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Template => _template != null ? KV("TEMPLATE", _template.AsTemplateStrings) : null;
  }
}
