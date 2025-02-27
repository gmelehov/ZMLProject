using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Models.Plugins;
using System.Diagnostics;






namespace ZMLProject.Models.Dockable
{
  /// <summary>
  /// Кнопка.
  /// </summary>
  public class Button : ZmlContainer, IButton, IHasContextBinders
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly List<IPlugin> _plugins;





    public Button(IZmlContainer host, XElement source) : base(host, source)
    {
      _items.AddRange(source.Elements().Select(el =>
      {
        var name = el.Name.LocalName;
        IZmlComponent ret;
        switch (name)
        {
          case "host.context": ret = new ContextBinder(this, el); break;


          default: ret = null; break;
        };
        return ret;
      }).Where(w => w != null));


      _plugins = new List<IPlugin>();

      if (ContextBinders.Any(a => a != null))
      {
        _plugins.Add(new HostContextPlugin(this));
      };



      _plugins.Add(new RipplePlugin(this));

    }







    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(
      KV_XType,
      KV_ItemId,
      KV("AppNAME", App.Name),
      KV_Text,
      KV_Call,
      KV_TargetId,
      KV_Align,
      KV_ColumnWidth,
      KV_IconSet,
      KV_Icon,
      KV_Color,
      KV_Padding,
      KV_Margin,
      KV_Disabled,
      KV_NeedSelection,
      KV_Plugins,
      App.bsrv.Button(this).OnClick,
      App.bsrv.Button(this).OnAfterClick
      );












    #region СВОЙСТВА //////////////////////////////////////////////////////////




    public string XType => App.bsrv.Button(this).XType;


    public string Id => this["id"];


    public string Call => this["call"];


    public string TargetId => this["targetId"];


    public string Text => this["text"];


    public string Icon => this["icon"];


    public string IconSet => App.bsrv.Button(this).IconSet;


    public string Color => this["color"];


    public string Align => this["align"];


    public string Margin => this["margin"];


    public string Padding => this["padding"];


    public double? ColumnWidth => parseDoubleAttr("columnWidth");


    public bool Disabled => App.bsrv.Button(this).Disabled;


    public string NeedSelection => this["needSelection"];


    public string AfterClick => this["onafterclick"];




    #endregion










    #region KEYVALUES /////////////////////////////////////////////////////////



    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_ItemId => KV("itemId", Id);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_XType => KV("xtype", XType);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Call => PROP("call", Call);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_TargetId => PROP("targetId", TargetId);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_IconSet => KV("iconSet", IconSet);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Text => PROP("text", Text);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Align => PROP("align", Align);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_ColumnWidth => PROP("columnWidth", ColumnWidth);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Icon => PROP("__icon", Icon);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Color => PROP("color", Color);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Margin => PROP("margin", Margin);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Padding => PROP("padding", Padding);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Disabled => KV("disabled", Disabled);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_NeedSelection => PROP("needSelection", NeedSelection);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Plugins => Plugins?.Count > 0 ? KV("plugins", Arr(false, true, Plugins.Select(s => s.AsConfig))) : null;


    #endregion












    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AbstractExpression Handler
    {
      get
      {
        switch (Call)
        {
          case "action": return App.jsrv.Action(TargetId).Call();
          case "source": return App.jsrv.Source(TargetId).Load();
          case "window": return App.jsrv.Dialog(TargetId).Show();


          default: return null;
        };
      }
    }







    public IList<IPlugin> Plugins => _plugins;

    public IEnumerable<IContextBinder> ContextBinders => _items.OfType<IContextBinder>();
  }
}
