using DiffCode.JSGenerator;
using DiffCode.CommonExtensions;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Models.Grids;
using System.Diagnostics;
using ZMLProject.Enums;



namespace ZMLProject.Models.Data;

/// <summary>
/// Базовое хранилище данных.
/// </summary>
[ManagedProps(PropAccess.Getter, ElProps.Value, ElProps.TotalCount, ElProps.TotalPages, ElProps.PageSize, ElProps.CurrentPage)]
[ManagedProps(PropAccess.Caller, "load")]
public abstract class BaseSource : ZmlContainer
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public readonly ZmlApplication _app;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected SourceModel _sourceModel;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected BaseProxy _sourceProxy;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected Grid _grid;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected Tree _tree;









  protected BaseSource(ZmlApplication app, XElement source) : base(app, source)
  {
    _app = app;

    var fields = source.Elements(e => e.Name.LocalName == "Field" || e.Name.LocalName == "Fields");
    fields?.ToList().ForEach(f =>
    {
      var name = f.Name.LocalName;
      BaseField fld;
      switch (name)
      {
        case "Field": fld = new Field(this, f); break;
        case "Fields": fld = new Fields(this, f); break;

        default: fld = null; break;
      };

      if (fld != null) _items.Add(fld);
    });

    _items.AddRange(source.Elements(F("Prm")).Select(s => new Param(this, s)).ToList());
    _items.AddRange(source.Elements(F("Item")).Select(s => new Item(this, s)).ToList());


    _sourceModel = new SourceModel(this);
  }









  public void ConnectToGrid(Grid grid) => _grid = grid;

  public void ConnectToTree(Tree tree) => _tree = tree;











  public override ExtJSClassExpression AsDefine { get; }


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override ObjectBody AsConfig => null;











  #region СВОЙСТВА //////////////////////////////////////////////////////////


  /// <summary>
  /// Идентификатор хранилища данных.
  /// </summary>
  public string Id => this["id"];

  /// <summary>
  /// Внешнее наименование хранилища данных.
  /// </summary>
  public string Name => this["name"] ?? Id;

  /// <summary>
  /// Тип провайдера данных.
  /// </summary>
  public virtual string Provider => this["provider"];

  /// <summary>
  /// Признак автоматической загрузки хранилища данных.
  /// </summary>
  public bool AutoLoad => App.bsrv.BaseSource(this).AutoLoad;

  /// <summary>
  /// Признак автоматической синхронизации состояния хранилища данных.
  /// </summary>
  public bool AutoSync => App.bsrv.BaseSource(this).AutoSync;

  /// <summary>
  /// Количество данных на странице.
  /// </summary>
  public int PageSize => App.bsrv.BaseSource(this).PageSize;

  /// <summary>
  /// Описание хранилища данных.
  /// </summary>
  public string Descr => this["descr"];

  /// <summary>
  /// Название ExtJS-класса, расширяемого определением этого хранилища.
  /// </summary>
  public abstract string Extend { get; }

  /// <summary>
  /// Название ExtJS-класса для этого хранилища.
  /// </summary>
  public string Classname => App.bsrv.BaseSource(this).Classname;

  /// <summary>
  /// Название ExtJS-класса для модели данных этого хранилища.
  /// </summary>
  public string ModelClassname => _sourceModel?.Classname;

  /// <summary>
  /// Название ExtJS-класса, расширяемого определением модели данных этого хранилища.
  /// </summary>
  public string ModelExtendname => _sourceModel?.Extend;

  /// <summary>
  /// Имя первичного ключа модели данных.
  /// </summary>
  public string Key => Fields?.FirstOrDefault(f => f.IsKey == true)?.Id ?? this["key"];

  /// <summary>
  /// Имя поля модели данных, по которому производится группировка данных.
  /// </summary>
  public string GroupBy => Fields?.FirstOrDefault(f => f.GroupBy == true)?.Id;





  /// <summary>
  /// Список полей модели данных для этого хранилища.
  /// </summary>
  public IEnumerable<Field> Fields => Items?.OfType<BaseField>().SelectMany(s =>
  {
    if (s is Field) return new Field[] { s.As<Field>() };
    if (s is Fields) return s.As<Fields>()?.Items?.Cast<Field>();
    return null;
  }).Where(w => w != null);

  /// <summary>
  /// Список столбцов с данными, сгенерированный по модели данных этого хранилища.
  /// </summary>
  public IEnumerable<ObjectBody> Columns => Fields?.Select(s => s.Column).Where(w => w != null);

  /// <summary>
  /// Список параметров для хранилища данных.
  /// </summary>
  public IEnumerable<Param> Params => _items.OfType<Param>();

  /// <summary>
  /// Локальные данные, определенные при создании этого хранилища.
  /// </summary>
  public IEnumerable<Item> DataItems => _items.OfType<Item>();







  /// <summary>
  /// Ссылка на связанную таблицу с древовидным отображением данных.
  /// </summary>
  public Tree Tree => _tree;



  #endregion






  #region KEYVALUES /////////////////////////////////////////////////////////




  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue ColumnsProperty =>
    Columns.Count() > 0
    ?
    KV("Columns",
      OBJ(
        KV("defaults", OBJ(false, false, KV("filterable", TRUE), KV("sortable", TRUE))),
        KV("items", Arr(false, Columns.Count() > 1, Columns))
        ))
    :
    null
    ;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Extend => PROP("extend", Extend);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Model => PROP("model", ModelClassname);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Id => PROP("storeId", Id);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_SourceId => PROP("sourceId", Name);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_ObjectType => PROP("ObjectType", "source");


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Key => PROP("KEY", Key);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Proxy => _sourceProxy != null ? KV("proxy", _sourceProxy.AsConfig) : null;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_PageSize => KV("pageSize", PageSize);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_AutoLoad => KV("autoLoad", AutoLoad);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_AutoDestroy => KV("autoDestroy", false);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_AutoSync => KV("autoSync", AutoSync);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_GroupField => PROP("groupField", GroupBy);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_TREE => _tree != null ? KV("TREE", Tree.Id) : null;







  #region СОБЫТИЯ ///////////////////////////////////////////////////////////


  /// <summary>
  /// Привязка обработчика события onload.
  /// </summary>
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_OnAfterLoad => App.bsrv.BaseSource(this).OnAfterLoad;


  /// <summary>
  /// Привязка обработчика события onwrite.
  /// </summary>
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_OnAfterWrite => App.bsrv.BaseSource(this).OnAfterWrite;


  #endregion



  #endregion








  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public AbstractExpression[] RelatedClasses => new AbstractExpression[] { _sourceModel.AsDefine, NEWLINES(0), AsDefine, NEWLINES(4) };

}
