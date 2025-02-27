using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;
using ZMLProject.Models.Plugins;
using DiffCode.CommonExtensions.Attributes;





namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Базовый элемент ввода данных.
  /// </summary>
  public abstract class BaseFormField : BaseFormItem, IFormItem, IHasId, IHasXType, IHasContextBinders, IPluginHost
  {
    protected readonly List<IPlugin> _plugins;





    protected BaseFormField(IZmlContainer host, XElement source) : base(host, source) 
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
    }











    public override ObjectBody AsConfig => OBJ(
      KV_XType,
      KV_Name,
      KV_ItemId,
      KV("AppNAME", App.Name),
      KV_Width,
      KV_Height,
      KV_MaxHeight,
      KV_MinHeight,
      KV_MaxWidth,
      KV_MinWidth,
      KV_Padding,
      KV_Margin,
      KV_Flex,
      KV_ColumnWidth,
      KV_Anchor,
      KV_Cls,
      KV_Disabled,
      KV_AllowBlank,
      KV_LabelAlign,
      KV_LabelClsExtra,
      KV_LabelWidth,
      KV_ReadOnly,
      KV_Region,
      KV_Tooltip,
      KV_AutoSync,
      KV_Plugins,
      App.bsrv.BaseFormField(this).ApiOnChange

      ) + SpecificKeyValues?.Props;


    protected abstract ObjectBody SpecificKeyValues { get; }












    public virtual string Id => this["id"];


    public abstract string XType { get; }


    public virtual string Name => this["name"];


    public virtual bool? AutoSync => parseBoolAttr("autoSync");


    public virtual bool? ReadOnly => parseBoolAttr("readOnly");


    public virtual string Region => this["region"];


    public virtual string Tooltip => this["tooltip"];














    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_ItemId => KV("itemId", Id);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_XType => KV("xtype", XType);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Name => PROP("fieldLabel", Name);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_AutoSync => PROP("autoSync", AutoSync);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_ReadOnly => PROP("readOnly", ReadOnly);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Region => PROP("region", Region);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Tooltip => PROP("tooltip", Tooltip);


    /// <summary>
    /// Автоматический расчет полей элемента внутри его родительского элемента.
    /// </summary>
    [ToDo("Вынести hardcoded в инициализаторы")]
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Margin =>
      Margin != null ? KV("margin", Margin)
      :
      HasFieldSetSiblings ? NextIsFieldSet ? KV("margin", "0 10 10 10") : KV("margin", "0 10 5 10")
      :
      null
      ;











    /// <summary>
    /// Признак того, что у этого поля ввода есть соседи по родительскому диалогу/форме, являющиеся группой полей ввода данных.
    /// Признак используется для автоматического расчета полей этого элемента внутри родительского элемента.
    /// </summary>
    public virtual bool HasFieldSetSiblings => Host.As<Form>() != null && Host.As<Form>().HasFieldSets;


    /// <summary>
    /// Признак того, что следующий за этим полем ввода элемент является группой полей ввода данных.
    /// Признак используется для автоматического расчета полей этогор элемента внутри родительского элемента.
    /// </summary>
    public virtual bool NextIsFieldSet => HasFieldSetSiblings && (Host.As<Form>()?.Items.SkipWhile(s => s != this).Skip(1).FirstOrDefault() is Inputs);








    public IList<IPlugin> Plugins => _plugins;

    public IEnumerable<IContextBinder> ContextBinders => _items.OfType<IContextBinder>();


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Plugins => Plugins?.Count > 0 ? KV("plugins", Arr(Plugins.Select(s => s.AsConfig))) : null;
  }
}
