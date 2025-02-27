using DiffCode.CommonExtensions;
using System.Xml.Linq;
using static DiffCode.JSGenerator.Generator;
using DiffCode.JSGenerator;
using System.Diagnostics;
using ZMLProject.Enums;


namespace ZMLProject.Models.Data;

/// <summary>
/// Базовое действие.
/// </summary>
[ManagedProps(PropAccess.Caller, "call")]
public abstract class BaseAction : ZmlContainer
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public readonly ZmlApplication _app;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected List<string> _fieldNames;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected BaseProxy _actionProxy;








  protected BaseAction(ZmlApplication host, XElement source) : base(host, source)
  {
    _app = host;

    var fields = source.Elements(F("Field"));
    _fieldNames = fields.Select(s => s.AttrVal("id")).ToList();

    var elems = source.Elements().ToList();
    elems.ForEach(b =>
    {
      IZmlComponent elem = null;
      switch (b.Name.LocalName)
      {
        case "Prm": elem = new Param(this, b); break;
        case "console.dir": elem = new ConsoleDir(this, b); break;
        case "call.action": elem = new CallAction(this, b); break;
        case "call.dialog": elem = new CallDialog(this, b); break;
        case "call.source": elem = new CallSource(this, b); break;
        case "clear.input": elem = new ClearInput(this, b); break;
        case "clear.editor": elem = new ClearEditor(this, b); break;
        case "create.record": elem = new CreateRecord(this, b); break;
        case "load.component": elem = new LoadComponent(this, b); break;
        case "reset.component": elem = new ResetComponent(this, b); break;
        case "set.prop": elem = new SetProp(this, b) { HostResult = ID("data") }; break;
        case "add.value": elem = new ValueAdder(this, b); break;
        case "remove.value": elem = new ValueRemover(this, b); break;


        default: break;
      };
      if (elem != null) _items.Add(elem);
    });

  }









  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override ExtJSClassExpression AsDefine => null;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override ObjectBody AsConfig => null;


  public abstract AbstractExpression AsCreate { get; }










  #region СВОЙСТВА //////////////////////////////////////////////////////////



  /// <summary>
  /// Идентификатор действия.
  /// </summary>
  public string Id => this["id"];

  /// <summary>
  /// Внешнее наименование действия.
  /// </summary>
  public string Name => this["name"] ?? Id;

  /// <summary>
  /// Описание действия.
  /// </summary>
  public string Descr => this["descr"];

  /// <summary>
  /// 
  /// </summary>
  public string Classname => App.bsrv.BaseAction(this).Classname;

  /// <summary>
  /// Схема выполнения действия (локальное/удаленный запрос/и т.п.).
  /// </summary>
  public string Run => this["run"];

  /// <summary>
  /// Текст подтверждения выполнения, выводимый в инфо-окне при запуске действия.
  /// </summary>
  public string ConfirmText => this["confirmText"];



  #endregion









  #region KEYVALUES /////////////////////////////////////////////////////////



  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue KV_Fields => KV("fields", Arr(_fieldNames.Select(s => Str(s))));


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue KV_Proxy => _actionProxy != null ? KV("proxy", _actionProxy.AsConfig) : null;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue KV_Commands => Commands?.Count() > 0 ? KV("commands", Arr(Commands)) : null;


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue KV_Paramlinkers => KV("paramLinkers", Arr(false, true, Params.Select(s => s.AsExprs)));









  #region СОБЫТИЯ ///////////////////////////////////////////////////////////


  /// <summary>
  /// Создание ссылки на действие, выполняемое при подтверждении инфо-окна с ошибкой.
  /// </summary>
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue OnAccepted => App.bsrv.BaseAction(this).OnAccepted;

  /// <summary>
  /// Создание ссылки на действие, выполняемое при отмене инфо-окна с ошибкой.
  /// </summary>
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue OnRejected => App.bsrv.BaseAction(this).OnRejected;

  /// <summary>
  /// 
  /// </summary>
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected KeyValue OnAfterLoad => App.bsrv.BaseAction(this).OnAfterLoad;


  #endregion



  #endregion







  protected IEnumerable<Param> Params => _items.OfType<Param>();


  public IEnumerable<AbstractExpression> Commands => _items.OfType<BaseCommand>()?.Select(s => s.AsExprs);

}
