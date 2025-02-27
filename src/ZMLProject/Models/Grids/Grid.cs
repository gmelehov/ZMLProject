using DiffCode.CommonExtensions;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Plugins;
using System.Xml.Linq;
using System.ComponentModel;
using ZMLProject.Enums;






namespace ZMLProject.Models.Grids
{
  /// <summary>
  /// Таблица данных.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Key, ElProps.Src, ElProps.Value, ElProps.SelCount, ElProps.SelectedKey)]
  public class Grid : BasePanel, IHasXType, IHasPlugins
  {
    private GridView _gridview;
    private GridPaging _gridpaging;
    private ContextMenu _ctxmenu;






    public Grid(IZmlContainer host, XElement source) : base(host, source)
    {
      if (Source != null)
      {
        Source.ConnectToGrid(this);
      };

      ProcessDockedItems(source);
      AddCommonPlugins();

      _gridview = new GridView(this);
      _gridpaging = new GridPaging(this);
      _ctxmenu = source?.Element(F("Context.menu")) != null ? new ContextMenu(this, source?.Element(F("Context.menu"))) : null;
    }






    /// <summary>
    /// Создает и подключает набор стандартных плагинов таблицы данных.
    /// </summary>
    private void AddCommonPlugins()
    {
      _plugins.Add(new GridEditorPlugin(this));

      if (IsEditable)
      {
        _plugins.Add(new GridEditingPlugin(this));
      };
    }







    public override ExtJSClassExpression AsDefine => null;
    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_AppNAME,
      KV_SelModel,
      KV_Store,
      KV_HidePaging,
      KV_HideHeaders,
      KV_ColumnLines,
      KV_RowLines,
      KV_ForceFit,
      KV_ViewConfig,
      _ctxmenu?.KV_CtxMenuTitle,
      _ctxmenu?.KV_CtxMenuItems,
      KV_Features,
      KV_Plugins,
      KV_Columns,
      App.bsrv.Grid(this).OnRowSelected,
      App.bsrv.Grid(this).OnRowDeselected,
      App.bsrv.Grid(this).OnRowDblClicked,
      App.bsrv.Grid(this).OnDataLoaded,
      App.bsrv.Grid(this).OnCtxMenu
      );













    /// <summary>
    /// 
    /// </summary>
    public override string XType => App.bsrv.Grid(this).XType;
    
    /// <summary>
    /// 
    /// </summary>
    public string Src => this["src"];
    
    /// <summary>
    /// Идентификатор действия, выполняемого при нажатии кнопки-иконки "Создать" в пейджинг-тулбаре.
    /// </summary>
    public string Create => this["create"] ?? (IsCrud ? Src : null);

    /// <summary>
    /// Идентификатор действия, выполняемого при нажатии кнопки-иконки "Сохранить изменения" в пейджинг-тулбаре.
    /// </summary>
    public string Update => this["update"] ?? (IsCrud ? Src : null);

    /// <summary>
    /// Идентификатор действия, выполняемого при нажатии кнопки-иконки "Удалить" в пейджинг-тулбаре.
    /// </summary>
    public string Delete => this["delete"] ?? (IsCrud ? Src : null);

    /// <summary>
    /// Признак поддержки операций с буфером обмена ("Копировать"/"Вставить").
    /// </summary>
    [DefaultValue(false)]
    public bool Clipboard => App.bsrv.Grid(this).Clipboard;

    /// <summary>
    /// Пейджинг-панель по умолчанию скрыта?
    /// </summary>
    public bool? HidePaging => parseBoolAttr("hidePaging");

    /// <summary>
    /// Заголовки столбцов таблицы данных по умолчанию скрыты?
    /// </summary>
    public bool? HideHeaders => parseBoolAttr("hideHeaders");

    /// <summary>
    /// Вертикальные разделительные линии в таблице данных по умолчанию скрыты?
    /// </summary>
    public bool? ColumnLines => parseBoolAttr("columnLines");

    /// <summary>
    /// Горизонтальные разделительные линии в таблице данных по умолчанию скрыты?
    /// </summary>
    public bool? RowLines => parseBoolAttr("rowLines");

    /// <summary>
    /// Четные/нечетные строки таблицы по умолчанию имеют различный вид?
    /// </summary>
    public bool StripeRows => App.bsrv.Grid(this).StripeRows;

    /// <summary>
    /// 
    /// </summary>
    public bool? ForceFit => parseBoolAttr("forceFit");

    /// <summary>
    /// Таблица данных поддерживает текстовое выделение строк?
    /// </summary>
    public bool TextSelection => App.bsrv.Grid(this).TextSelection;

    /// <summary>
    /// Разрешено перетаскивание и переупорядочивание строк в таблице данных?
    /// </summary>
    public bool RowsDragDrop => App.bsrv.Grid(this).RowsDragDrop;

    /// <summary>
    /// Тип выделения строк в таблице.
    /// </summary>
    public string Sel => App.bsrv.Grid(this).Sel;

    /// <summary>
    /// Способ редактирования данных в таблице (отдельная ячейка/вся строка).
    /// </summary>
    public string Editor => App.bsrv.Grid(this).Editor;

    /// <summary>
    /// Идентификатор плагина для инлайн-редактирования в таблице данных.
    /// </summary>
    public string EditorId => App.bsrv.Grid(this).EditorId;

    /// <summary>
    /// 
    /// </summary>
    public string SelMode => App.bsrv.Grid(this).SelMode;

    /// <summary>
    /// 
    /// </summary>
    public string SelectionModel => App.bsrv.Grid(this).SelectionModel;

    /// <summary>
    /// 
    /// </summary>
    public bool AllowDeselect => true;

    /// <summary>
    /// 
    /// </summary>
    public override string Layout => null;

    /// <summary>
    /// 
    /// </summary>
    public BaseSource Source => Closest<ZmlApplication>()?.GetSource(Src);
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsCrud => Source.As<CrudSource>()?.Crud ?? false;
    
    /// <summary>
    /// Признак редактируемой таблицы данных.
    /// Вычисляется автоматически, исходя из присутствия в модели данных 
    /// хотя бы одного поля, для которого задан тип редактора значений.
    /// </summary>
    public bool IsEditable => Source?.Fields.Any(a => a.Editor != null) ?? false;

















    public override KeyValue KV_XType => KV("xtype", XType);
    
    
    public KeyValue KV_Store => KV("store", Src);
    
    
    public KeyValue KV_Columns =>
      KV("columns",
        OBJ(
          KV("defaults", OBJ(false, false, KV("filterable", TRUE), KV("sortable", TRUE))),
          KV("items", Arr(false, Source?.Columns.Count() > 1, Source?.Columns))
          ));
    
    
    public override KeyValue KV_Items => null;
    
    
    public override KeyValue KV_Docked => KV("dockedItems",
      Docked != null ?
      Arr(false, true,
        Docked?.Forms?.Select(t => t?.AsConfig).Concat(Docked?.Toolbars?.Select(t => t?.AsConfig)) +
        _gridpaging.AsConfig
        )
      :
      Arr(false, false, _gridpaging.AsConfig)
      );
    
    
    public KeyValue KV_SelModel => SelMode != null ?
      KV("selModel",
        OBJ(false, false,
          KV("selType", SelectionModel),
          KV("allowDeselect", AllowDeselect),
          KV("mode", SelMode)
          )
        )
      : null;
    
    
    public override KeyValue KV_Layout => null;
    
    
    public KeyValue KV_HidePaging => PROP("hidePaging", HidePaging);


    public KeyValue KV_HideHeaders => PROP("hideHeaders", HideHeaders);
    
    
    public KeyValue KV_ColumnLines => PROP("columnLines", ColumnLines);
    
    
    public KeyValue KV_RowLines => PROP("rowLines", RowLines);
    
    
    public KeyValue KV_ForceFit => PROP("forceFit", ForceFit);
    
    
    public KeyValue KV_ViewConfig => KV("viewConfig", _gridview?.AsConfig);


    private KeyValue KV_AppNAME => KV("AppNAME", App.Name);










    






    /// <summary>
    /// Все заданные опции таблицы данных.
    /// </summary>
    private KeyValue KV_Features => FeaturesArray.Count() > 0 ? KV("features", Arr(false, false, FeaturesArray)) : null;

    /// <summary>
    /// Признак наличия в источнике данных поля с выводом суммарного значения.
    /// </summary>
    private bool HasSummary => Source != null && Source.Fields.Any(a => a.SummaryType.IsUseful());

    /// <summary>
    /// Признак наличия в источнике данных поля с группировкой значений.
    /// </summary>
    private bool HasGrouping => Source != null && Source.Fields.Any(a => a.GroupBy);

    /// <summary>
    /// Опция таблицы данных, включающая вывод суммарных значений.
    /// Включена, если в источнике данных есть хотя бы одно поле с выводом суммарного значения.
    /// </summary>
    private ObjectBody KVSummaryFeature => (HasSummary && !HasGrouping) ? OBJ(false, false, KV("ftype", "summary"), KV("dock", "top")) : null;

    /// <summary>
    /// Опция таблицы данных, включающая вывод сгруппированных значений.
    /// Включена, если в источнике данных есть хотя бы одно поле с группировкой значений.
    /// </summary>
    private ObjectBody KVGroupingFeature => (HasGrouping && !HasSummary) ? OBJ(false, false, KV("ftype", "grouping")) : null;

    /// <summary>
    /// Опция таблицы данных, включающая вывод сгруппированных значений с подсчетом суммарных значений по группе.
    /// Включена, если в источнике данных есть хотя бы одно поле с группировкой значений и хотя бы одно поле с выводом суммарных значений.
    /// </summary>
    private ObjectBody KVGroupingSummaryFeature => (HasGrouping && HasSummary) ? OBJ(false, false, KV("ftype", "groupingsummary"), KV("startCollapsed", TRUE)) : null;

    /// <summary>
    /// 
    /// </summary>
    private ObjectBody KVFilteringFeature => OBJ(false, false, KV("ftype", "filters"), KV("encode", true));

    /// <summary>
    /// Приватный список всех непустых опций таблицы данных.
    /// </summary>
    private IEnumerable<ObjectBody> FeaturesArray => new ObjectBody[] { KVSummaryFeature, KVGroupingFeature, KVGroupingSummaryFeature, KVFilteringFeature };

  }
}
