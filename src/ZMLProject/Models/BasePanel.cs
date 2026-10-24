using DiffCode.CommonExtensions;
using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Plugins;
using ZMLProject.Models.Grids;
using ZMLProject.Models.Dockable;
using ZMLProject.Models.Forms;
using ZMLProject.Models.Charts;
using System.Diagnostics;


namespace ZMLProject.Models;

/// <summary>
/// Базовая панель.
/// </summary>
public abstract class BasePanel : ZmlContainer, IBasePanel
{
  protected List<IPlugin> _plugins = new List<IPlugin>();






  protected BasePanel(IZmlContainer host, XElement source) : base(host, source)
  {
    ProcessFixedSizes();

    _plugins.Add(new ColorManagerPlugin(this));
  }








  /// <summary>
  /// Обработка отсутствующих в разметке наиболее употребительных атрибутов.
  /// </summary>
  public virtual void ProcessCommonAttributes()
  {
    if (!_source.HasAttr("title"))
    {
      _source.SetAttributeValue("collapsible", false);
      _source.SetAttributeValue("titleCollapse", false);
      _source.SetAttributeValue("split", false);
      _source.SetAttributeValue("header", false);
    }
    else
    {
      if (_source.HasAttr("collapseTo"))
      {
        if (!_source.HasAttr("collapsible")) _source.SetAttributeValue("collapsible", true);
        if (!_source.HasAttr("titleCollapse")) _source.SetAttributeValue("titleCollapse", true);
        if (!_source.HasAttr("split")) _source.SetAttributeValue("split", true);
      };
    };
    if (!_source.HasAttr("layout")) _source.SetAttributeValue("layout", "fit");
    if (!_source.HasAttr("width")) _source.SetAttributeValue("width", "100%");
    if (!_source.HasAttr("height")) _source.SetAttributeValue("height", "100%");
    if (!_source.HasAttr("flex") && _source.AttrVal("width") == "100%" && _source.AttrVal("height") == "100%") _source.SetAttributeValue("flex", 1);
    if (!_source.HasAttr("border")) _source.SetAttributeValue("border", 0);
    if (!_source.HasAttr("bodyBorder")) _source.SetAttributeValue("bodyBorder", 0);

  }

  /// <summary>
  /// Обработка атрибутов fixWidth, fixHeight.
  /// </summary>
  public virtual void ProcessFixedSizes()
  {
    if (_source.HasAttr("fixWidth"))
    {
      _source.SetAttributeValue("minWidth", _source.AttrVal("fixWidth"));
      _source.SetAttributeValue("maxWidth", _source.AttrVal("fixWidth"));
      _source.SetAttributeValue("width", _source.AttrVal("fixWidth"));
    };
    if (_source.HasAttr("fixHeight"))
    {
      _source.SetAttributeValue("minHeight", _source.AttrVal("fixHeight"));
      _source.SetAttributeValue("maxHeight", _source.AttrVal("fixHeight"));
      _source.SetAttributeValue("height", _source.AttrVal("fixHeight"));
    };
  }

  /// <summary>
  /// Обработка дочерних элементов, пристыкованных к этой панели.
  /// </summary>
  /// <param name="source"></param>
  public virtual void ProcessDockedItems(XElement source)
  {
    var dockeditems = source?.Element(F("Docked"));
    if (dockeditems != null)
    {
      _docked = new Docked(this, dockeditems);
    };
  }

  /// <summary>
  /// Автоматическое добавление сплиттеров между дочерними контейнерами.
  /// </summary>
  /// <param name="source"></param>
  /// <returns></returns>
  public virtual XElement AutoAddSplitters(XElement source)
  {
    if (this["layout"] != "accordion" && this["layout"] != "border" && (this["autoSplitters"] == null || parseBoolAttr("autoSplitters") == true))
    {
      var okList = new string[] { "Panel", "Tabpanel", "Grid", "Area", "Frame", "Editor", "List", "Tree", "Form", "Chart", "Component" };
      var stopList = new string[] { "Dialog", "host.context", "Docked" };
      source
        .Elements(w => okList.Contains(w.Name.LocalName))
        .Where(ww => ww.CountSiblingsAfter() > 0 && !stopList.Contains(ww.NextElement().Name.LocalName))
        .ToList()
        .ForEach(f =>
        {
          var splitter = new XElement(XName.Get("Splitter", "ZMLAppSchema"), new XAttribute("collapseTarget", App.bsrv.Splitter(null).CollapseTarget_Default));
          splitter.SetAttributeValue("size", SplitterSize);
          if (SplitterHover) splitter.SetAttributeValue("overCls", App.bsrv.Splitter(null).OverCls_Default);
          f.AddAfterSelf(splitter);
        });
    };
    return source;
  }

  /// <summary>
  /// Обработка вложенных дочерних элементов.
  /// </summary>
  /// <param name="source"></param>
  public virtual void ProcessCommonElements(XElement source)
  {
    _items.AddRange(source.Elements().Select(el =>
    {
      IZmlComponent ret = el.Name.LocalName switch
      {
        "Tabpanel" => new Tabpanel(this, el),
        "Panel" => new Panel(this, el),
        "Grid" => new Grid(this, el),
        "Form" => new Form(this, el),
        "Editor" => new Editor(this, el),
        "List" => new List(this, el),
        "Splitter" => new Splitter(this, el.Cut()),
        "Tree" => new Tree(this, el),
        "Chart" => new Chart(this, el),
        _ => null,
      };
      return ret;
    })
    .Where(w => w != null));
  }










  public override ObjectBody AsConfig => OBJ(
    KV_XType,
    KV_ItemId,
    KV_Title,
    KV_Header,
    KV_BodyCls,
    KV_Cls,
    KV_Width,
    KV_Height,
    KV_MinWidth,
    KV_MaxWidth,
    KV_MinHeight,
    KV_MaxHeight,
    KV_Flex,
    KV_Anchor,
    KV_Disabled,
    KV_OverflowX,
    KV_OverflowY,
    KV_Region,
    KV_Border,
    KV_BodyBorder,
    KV_BodyPadding,
    KV_Margin,
    KV_Padding,
    KV_Layout,
    KV_CollapseTo,
    KV_Collapsible,
    KV_TitleCollapse,
    KV_Split
    )
    +
    SpecificKeyValues?.Props
    +
    KV_Items
    +
    KV_Docked
    ;
  
  protected abstract ObjectBody SpecificKeyValues { get; }











  public virtual string Id => this["id"];
  
  public abstract string XType { get; }
  
  public virtual string Title => this["title"];
  
  public virtual object Header => this["header"];
  
  public virtual string BodyCls => this["bodyCls"];
  
  public virtual string Cls => this["cls"];
  
  public virtual double? Flex => parseDoubleAttr("flex");
  
  public string Anchor => this["anchor"];
  
  public bool? Disabled => parseBoolAttr("disabled");
  
  public string OverflowX => this["overflowX"];
  
  public string OverflowY => this["overflowY"];
  
  public string Margin => this["margin"];
  
  public string Padding => this["padding"];
  
  public virtual string Width => this["width"];
  
  public virtual string Height => this["height"];
  
  public virtual string MinWidth => this["minWidth"];
  
  public virtual string MaxWidth => this["maxWidth"];
  
  public virtual string MinHeight => this["minHeight"];
  
  public virtual string MaxHeight => this["maxHeight"];
  
  public string Region => this["region"];
  
  public virtual int? Border => parseIntAttr("border");
  
  public virtual int? BodyBorder => parseIntAttr("bodyBorder");
  
  public string BodyPadding => this["bodyPadding"];
  
  public string CollapseTo => this["collapseTo"];
  
  public bool? Collapsible => parseBoolAttr("collapsible");
  
  public virtual bool? TitleCollapse => parseBoolAttr("titleCollapse");
  
  public virtual bool? Split => parseBoolAttr("split");

  public virtual bool SplitterHover => App.bsrv.BasePanel(this).SplitterHover;

  public virtual int SplitterSize => App.bsrv.BasePanel(this).SplitterSize;

  public abstract string Layout { get; }

  public virtual string TitleColor => App.bsrv.BasePanel(this).TitleColor;

  public virtual string TitleBackColor => App.bsrv.BasePanel(this).TitleBackColor;

  public virtual string BodyBackColor => App.bsrv.BasePanel(this).BodyBackColor;










  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public abstract KeyValue KV_XType { get; }


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public abstract KeyValue KV_Layout { get; }


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_ItemId => PROP("itemId", Id);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Title => PROP("title", Title);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Header => PROP("header", Header);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_BodyCls => PROP("bodyCls", BodyCls);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Cls => PROP("cls", Cls);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Flex => PROP("flex", Flex);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Anchor => PROP("anchor", Anchor);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Disabled => PROP("disabled", Disabled);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_OverflowX => PROP("overflowX", OverflowX);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_OverflowY => PROP("overflowY", OverflowY);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Margin => PROP("margin", Margin);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Padding => PROP("padding", Padding);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Width => PROP("width", Width);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Height => PROP("height", Height);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_MinWidth => PROP("minWidth", MinWidth);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_MaxWidth => PROP("maxWidth", MaxWidth);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_MinHeight => PROP("minHeight", MinHeight);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_MaxHeight => PROP("maxHeight", MaxHeight);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Region => PROP("region", Region);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Border => PROP("border", Border);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_BodyBorder => PROP("bodyBorder", BodyBorder);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_BodyPadding => PROP("bodyPadding", BodyPadding);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_CollapseTo => PROP("collapseDirection", CollapseTo);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Collapsible => PROP("collapsible", Collapsible);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_TitleCollapse => PROP("titleCollapse", TitleCollapse);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Split => PROP("split", Split);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Items => _items.Count > 0 ? KV("items", Arr(false, true, _items?.Select(f => f.AsConfig))) : null;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Docked => Docked?.Items.Count > 0 ? KV("dockedItems", Arr(false, true, Docked?.Toolbars.Select(t => t.AsConfig))) : null;








  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public virtual KeyValue KV_Plugins => KV("plugins", Arr(false, false, Plugins.Select(s => s.AsConfig)));
  
  
  public IList<IPlugin> Plugins => _plugins;

}
