using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Enums;
using System.Diagnostics;

namespace ZMLProject.Models;

/// <summary>
/// Универсальная панель.
/// </summary>
[ManagedProps(PropAccess.Getter, ElProps.Collapsed, ElProps.Loader)]
[ManagedProps(PropAccess.Setter, ElProps.Collapsed, ElProps.Loader)]
[ManagedProps(PropAccess.ValueAdder, ElProps.Cls, ElProps.BodyCls)]
[ManagedProps(PropAccess.ValueRemover, ElProps.Cls, ElProps.BodyCls)]
public class Panel : BasePanel, IDockable
{
  public Panel(ZmlContainer host, XElement source) : base(host, source)
  {

    ProcessCommonAttributes();

    AutoAddSplitters(source);

    ProcessCommonElements(source);

    ProcessDockedItems(source);
  }









  public override ExtJSClassExpression AsDefine => null;
  protected override ObjectBody SpecificKeyValues => OBJ(
    KV_AppNAME,
    KV_Loader,
    KV_Dock,
    KV_Plugins
    );










  /// <summary>
  /// 
  /// </summary>
  public override string XType => App.bsrv.Panel(this).XType;

  /// <summary>
  /// 
  /// </summary>
  public override string Layout => App.bsrv.Panel(this).Layout;

  /// <summary>
  /// 
  /// </summary>
  public string Dock => this["dock"];

  /// <summary>
  /// 
  /// </summary>
  public string Loader => this["loader"];










  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override KeyValue KV_XType => KV("xtype", XType);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override KeyValue KV_Layout =>
    Layout == "accordion" ?
    KV("layout", OBJ(false, false,
      KV("type", "accordion"),
      KV("multi", this["layout"] == "multiaccordion"),
      KV("animate", TRUE)
      ))
    :
    PROP("layout", this["layout"]);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Dock => PROP("dock", Dock);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Loader => App.HasLoader(Loader) ? PROP("__loader", Loader) : null;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private KeyValue KV_AppNAME => KV("AppNAME", App.Name);

}
