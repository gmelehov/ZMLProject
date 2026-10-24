using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Diagnostics;
using ZMLProject.Enums;


namespace ZMLProject.Models
{
  /// <summary>
  /// Редактор кода.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.HtmlContent, ElProps.Mode, ElProps.Value)]
  [ManagedProps(PropAccess.Setter, ElProps.Mode, ElProps.Value)]
  [ManagedProps(PropAccess.Caller, "Clear")]
  public class Editor : BasePanel
  {
    public Editor(ZmlContainer host, XElement source) : base(host, source)
    {
      ProcessCommonAttributes();
    }


    public override ExtJSClassExpression AsDefine => null;
    protected override ObjectBody SpecificKeyValues => OBJ(false, false,
      KV_AppNAME,
      KV_Mode,
      KV_Theme,
      KV_TabSize,
      KV_LineNumbers,
      KV_LineWrapping,
      App.bsrv.Editor(this).OnAfterChanged
      );


    public override string XType => App.bsrv.Editor(this).XType;
    public string Mode => this["mode"];
    public string Theme => this["theme"];
    public int? TabSize => parseIntAttr("tabSize");
    public bool? LineNumbers => parseBoolAttr("lineNumbers");
    public bool? LineWrapping => parseBoolAttr("lineWrap");
    public override string Layout => App.bsrv.Editor(this).Layout;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_XType => KV("xtype", XType);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Mode => KV("mode", Mode);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Theme => PROP("theme", Theme);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_TabSize => PROP("tabSize", TabSize);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_LineNumbers => PROP("lineNumbers", LineNumbers);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_LineWrapping => PROP("lineWrapping", LineWrapping);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Layout => KV("layout", Layout);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Items => null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Docked => null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_AppNAME => KV("AppNAME", App.Name);

  }
}
