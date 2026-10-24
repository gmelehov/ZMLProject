using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Diagnostics;
using ZMLProject.Enums;

namespace ZMLProject.Models;

/// <summary>
/// Список.
/// </summary>
[ManagedProps(PropAccess.Getter, ElProps.Value, ElProps.SelCount, ElProps.TotalCount)]
public class List : BasePanel
{
  protected Template _template;




  public List(IZmlContainer host, XElement source) : base(host, source)
  {
    var templSource = source.Element(F("Template"));
    if (templSource != null) _template = new Template(this, templSource);
    _items.AddRange(source.Elements(F("Item")).Select(s => new Item(this, s)));

    ProcessDockedItems(source);
  }




  public override ExtJSClassExpression AsDefine => null;
  
  protected override ObjectBody SpecificKeyValues => OBJ(
    KV_AppNAME,
    KV_Src,
    KV_Sel,
    KV_HideHeaders,
    KV_HidePaging,
    KV_PagingDock,
    KV_ItemSelector,
    KV_Plugins,
    KV_TemplateArray,
    KV_DataArray,
    KV_FieldsArray
    );




  /// <summary>
  /// 
  /// </summary>
  public override string XType => App.bsrv.List(this).XType;
  
  /// <summary>
  /// 
  /// </summary>
  public string Src => this["src"] != null ? $"{this["src"]}" : null;

  /// <summary>
  /// 
  /// </summary>
  public bool? HidePaging => App.bsrv.List(this).HidePaging;

  /// <summary>
  /// 
  /// </summary>
  public bool? HideHeaders => App.bsrv.List(this).HideHeaders;

  /// <summary>
  /// 
  /// </summary>
  public string Sel => App.bsrv.List(this).Sel;

  /// <summary>
  /// 
  /// </summary>
  public string PagingDock => App.bsrv.List(this).PagingDock;

  /// <summary>
  /// 
  /// </summary>
  public string ItemSelector => App.bsrv.List(this).ItemSelector;

  /// <summary>
  /// 
  /// </summary>
  public override string Layout => App.bsrv.List(this).Layout;









  public Template Template => _template;












  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override KeyValue KV_XType => KV("xtype", XType);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override KeyValue KV_Layout => KV("layout", Layout);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Src => PROP("src", Src);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Sel => KV("sel", Sel);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_HidePaging => PROP("hidePaging", HidePaging);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_HideHeaders => PROP("hideHeaders", HidePaging);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_PagingDock => KV("pagingDock", PagingDock);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_ItemSelector => PROP("itemSelector", ItemSelector);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_TemplateArray => KV("TEMPLATE", _template.AsTemplateStrings);

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_DataArray => KV("DATA", Arr(false, true, DataItems));

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_FieldsArray => KV("FIELDS", Arr(false, true, DataFields.Select(s => Str(s))));

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override KeyValue KV_Items => null;

  
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private KeyValue KV_AppNAME => KV("AppNAME", App.Name);








  private IEnumerable<ObjectBody> DataItems => _items.OfType<Item>()?.Select(s => s.AsConfig);

  private IEnumerable<string> DataFields => _template?.DataDict.Where(w => w.Key != "itemSelector").Select(s => s.Value);
}
