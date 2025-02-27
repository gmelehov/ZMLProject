using DiffCode.CommonExtensions;
using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Plugins;
using ZMLProject.Models.Forms;
using ZMLProject.Models.Grids;
using ZMLProject.Models.Charts;
using System.Diagnostics;
using ZMLProject.Enums;








namespace ZMLProject.Models
{
  /// <summary>
  /// Диалоговое окно.
  /// </summary>
  [ManagedProps(PropAccess.ValueAdder, ElProps.Cls, ElProps.BodyCls)]
  [ManagedProps(PropAccess.ValueRemover, ElProps.Cls, ElProps.BodyCls)]
  [ManagedProps(PropAccess.Caller, "show")]
  public class Dialog : BasePanel
  {

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private List<ContextBinder> _ctxbinders = new List<ContextBinder>();






    public Dialog(ViewPort host, XElement source) : base(host, source)
    {
      _ctxbinders.AddRange(source.Elements(F("host.context")).Select(el => new ContextBinder(this, el)).Where(w => w != null));

      if (source != null)
      {
        if (!_source.HasAttr("layout")) _source?.SetAttributeValue("layout", "fit");
        if (!_source.HasAttr("border")) _source?.SetAttributeValue("border", 0);
        if (!_source.HasAttr("bodyBorder")) _source?.SetAttributeValue("bodyBorder", 0);

        AutoAddSplitters(source);

        _items?.AddRange(source?.Elements().Where(ww => ww.Name.LocalName != "host.context").Select(el =>
        {
          var name = el.Name.LocalName;
          IZmlComponent ret;
          switch (name)
          {
            case "Editor": ret = new Editor(this, el); break;
            case "Form": ret = new Form(this, el); break;
            case "Grid": ret = new Grid(this, el); break;
            case "List": ret = new List(this, el); break;
            case "Panel": ret = new Panel(this, el); break;
            case "Splitter": ret = new Splitter(this, el.Cut()); break;
            case "Tabpanel": ret = new Tabpanel(this, el); break;
            case "Tree": ret = new Tree(this, el); break;
            case "Chart": ret = new Chart(this, el); break;


            default: ret = null; break;
          };
          return ret;
        })
        .Where(w => w != null));


        ProcessDockedItems(source);
      };
    }








    public override ExtJSClassExpression AsDefine => null;
    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_AppNAME,
      KV_Modal,
      KV_NoRipple,
      KV_StdButtons,
      KV_CtxBinders,
      KV_ConfirmAction,
      App.bsrv.Dialog(this).OnShow,
      App.bsrv.Dialog(this).OnCancel,
      App.bsrv.Dialog(this).OnAfterConfirm
      );









    /// <summary>
    /// 
    /// </summary>
    public override string XType => App.bsrv.Dialog(this).XType;

    /// <summary>
    /// 
    /// </summary>
    public override string Layout => App.bsrv.Dialog(this).Layout;

    /// <summary>
    /// 
    /// </summary>
    public bool StdButtons => App.bsrv.Dialog(this).StdButtons;

    /// <summary>
    /// 
    /// </summary>
    public bool Modal => App.bsrv.Dialog(this).Modal;

    /// <summary>
    /// 
    /// </summary>
    public string OnConfirm => this["onconfirm"];













    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_NoRipple => KV("noRipple", false);


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
      PROP("layout", this["layout"] ?? "fit");


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_StdButtons => KV("standardButtons", StdButtons);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_CtxBinders => KV("ContextBinders", Arr(_ctxbinders.Select(s => s.AsConfig)));


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_ConfirmAction => PROP("ConfirmAction", OnConfirm);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Modal => KV("modal", Modal);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_AppNAME => KV("AppNAME", App.Name);

  }
}
