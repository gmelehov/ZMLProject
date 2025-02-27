using DiffCode.CommonExtensions;
using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Forms;
using ZMLProject.Models.Grids;
using ZMLProject.Models.Charts;
using System.Diagnostics;






namespace ZMLProject.Models
{
  /// <summary>
  /// Основное представление (страница) приложения.
  /// </summary>
  public class ViewPort : BasePanel
  {
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected ZmlApplication _app;







    public ViewPort(ZmlApplication app, XElement source) : base(app, source)
    {
      _app = app;

      if (source != null)
      {
        AutoAddSplitters(source);

        _items?.AddRange(source?.Elements().Select(el =>
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
            case "Dialog": ret = new Dialog(this, el); break;
            case "Chart": ret = new Chart(this, el); break;


            default: ret = null; break;
          };
          return ret;
        })
        .Where(w => w != null));


        ProcessDockedItems(source);
      };
    }








    public override ExtJSClassExpression AsDefine => App.jsrv.Ext_define(Classname, OBJ(
      KV_Extend,
      KV_Alias,
      KV_ItemId,
      KV_Layout,
      KV_Border,
      KV_BodyBorder,
      KV_BodyPadding,
      KV_Title,
      KV_Padding,
      KV_Margin,
      KV_OverflowX,
      KV_OverflowY,
      KV_Width,
      KV_Height,
      KV_Items,
      KV_Docked
      ), "Класс основного представления приложения.");
    public ExtJSClassExpression AsCreate => App.jsrv.Ext_create(Extend, OBJ(
      KV_RenderTo,
      KV_ItemId,
      KV_Layout,
      KV_Border,
      KV_BodyBorder,
      KV_BodyPadding,
      KV_Title,
      KV_Padding,
      KV_Margin,
      KV_OverflowX,
      KV_OverflowY,
      KV_Width,
      KV_Height,
      KV_Listeners,
      KV_Items,
      KV_Docked
      ));
    public override ObjectBody AsConfig => null;
    protected override ObjectBody SpecificKeyValues => OBJ();












    /// <summary>
    /// 
    /// </summary>
    public override string Id => App.bsrv.ViewPort(this).Id;

    /// <summary>
    /// 
    /// </summary>
    public override string XType => null;

    /// <summary>
    /// 
    /// </summary>
    public string Classname => App.bsrv.ViewPort(this).Classname;

    /// <summary>
    /// 
    /// </summary>
    public string Extend => App.bsrv.ViewPort(this).Extend;

    /// <summary>
    /// 
    /// </summary>
    public string Alias => App.bsrv.ViewPort(this).Alias;

    /// <summary>
    /// 
    /// </summary>
    public override string Layout => App.bsrv.ViewPort(this).Layout;

    /// <summary>
    /// 
    /// </summary>
    public override string Title => this["title"];
    
    /// <summary>
    /// 
    /// </summary>
    public string Target => this["target"];
    
    /// <summary>
    /// 
    /// </summary>
    public override string Height => this["height"] ?? "100%";

    /// <summary>
    /// 
    /// </summary>
    public override string Width => this["width"] ?? "100%";

    /// <summary>
    /// 
    /// </summary>
    public override double? Flex => parseDoubleAttr("flex") ?? 1;

    /// <summary>
    /// 
    /// </summary>
    public override int? Border => parseIntAttr("border") ?? 0;

    /// <summary>
    /// 
    /// </summary>
    public override int? BodyBorder => parseIntAttr("bodyBorder") ?? 0;













    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Extend => KV("extend", Extend);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Alias => KV("alias", Alias);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_XType => null;


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
    public KeyValue KV_RenderTo => Target != null ? KV("renderTo", App.jsrv.Ext_Get(Target)) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Height => PROP("height", Height);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Width => PROP("width", Width);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Items => KV("items", ItemsArray);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Listeners => _app.Onload.IsUseful() ? KV("listeners", OBJ(
      KV("afterrender", FN(App.jsrv.Action(_app.Onload).Call()))
      )) : null;







    /// <summary>
    /// Если основная страница приложения рендерится в собственном контейнере (не на всю страницу),
    /// после запуска приложения необходимо задать обработчик события изменения размеров окна браузера.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AssignExpr ONResize => Target != null ? ASSIGN(MEMB(ID("window"), ID("onresize")), FN(App.jsrv.APP_VIEW_DoLayout())) : null;




    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private ArrayDefinition ItemsArray => Arr(false, true, Items.Where(w => !(w is Dialog)).Select(s => s.AsConfig));
    
  }
}
