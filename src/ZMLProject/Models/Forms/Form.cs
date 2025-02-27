using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;
using ZMLProject.Models.Dockable;
using System.Diagnostics;
using DiffCode.CommonExtensions.Attributes;
using ZMLProject.Enums;






namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Форма ввода данных.
  /// </summary>
  [ManagedProps(PropAccess.ValueAdder, ElProps.Cls, ElProps.BodyCls)]
  [ManagedProps(PropAccess.ValueRemover, ElProps.Cls, ElProps.BodyCls)]
  public class Form : BasePanel, IForm
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IFormDefaults _defaults;






    public Form(IZmlContainer host, XElement source) : base(host, source)
    {
      _items.AddRange(source?.Elements()?.AsEnumerable().Select(s =>
      {
        var name = s.Name.LocalName;
        IZmlComponent ret;
        switch (name)
        {
          case "Defaults": _defaults = new FormDefaults(this, s); ret = null; break;
          case "Inputs": ret = new Inputs(this, s); break;
          case "Combo": ret = new Combobox(this, s); break;
          case "Checkbox": ret = new Checkbox(this, s); break;
          case "NumberField": ret = new NumberField(this, s); break;
          case "NumberSet": ret = new NumberSet(this, s); break;
          case "DateField": ret = new DateField(this, s); break;
          case "DateSet": ret = new DateSet(this, s); break;
          case "TimeField": ret = new TimeField(this, s); break;
          case "MonthCombo": ret = new MonthCombo(this, s); break;
          case "TextField": ret = new TextField(this, s); break;
          case "TextArea": ret = new TextArea(this, s); break;
          case "KeyValueInput": ret = new KeyValueInput(this, s); break;
          //case "PairedCombo": ret = new 
          case "Collection": ret = new Collection(this, s); break;
          case "Button": ret = new Button(this, s); break;
          case "Form": ret = new Form(this, s); break;


          default: ret = null; break;
        };
        return ret;
      })
      .Where(w => w != null));


      ProcessDockedItems(source);
    }







    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override ExtJSClassExpression AsDefine => null;
    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_AppNAME,
      KV_FieldDefaults,
      KV_ColumnWidth,
      KV_AutoLoad,
      KV_Src,
      KV_Dock,
      KV_Plugins,
      KV_OnValid
      );












    public override string XType => App.bsrv.Form(this).XType;


    public bool? AutoLoad => App.bsrv.Form(this).AutoLoad;


    public double? ColumnWidth => App.bsrv.Form(this).ColumnWidth;


    public override string Width => App.bsrv.Form(this).Width;


    public override string Height => App.bsrv.Form(this).Height;


    public string Src => this["src"];


    public string Dock => this["dock"];
    
    
    public string OnValid => this["onvalid"];


    public bool HasFieldSets => Items?.OfType<Inputs>().Count() > 0;
    
    
    public IEnumerable<IButton> Buttons => _items.OfType<IButton>();


    public override string Layout => App.bsrv.Form(this).Layout;


    public override int? BodyBorder => App.bsrv.Form(this).BodyBorder;

















    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_XType => KV("xtype", XType);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Layout => KV("layout", Layout);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_AutoLoad => Src != null && Id != null ? PROP("autoload", AutoLoad) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Src => PROP("src", Src);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Dock => PROP("dock", Dock);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_ColumnWidth => KV("columnWidth", ColumnWidth);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_FieldDefaults => _defaults != null ? KV("fieldDefaults", _defaults.AsConfig) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_AppNAME => KV("AppNAME", App.Name);








    [ToDo("Прописать в билдере!")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_OnValid => 
      OnValid != null 
      ? 
      KV("OnValid", App.jsrv.Action(OnValid).Call())
      : 
      null;

  }
}
