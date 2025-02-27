using DiffCode.CommonExtensions;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;


namespace ZMLProject.Models;

/// <summary>
/// Базовый класс для компонентов разметки.
/// </summary>
public abstract class ZmlComponent : IZmlComponent
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected IZmlContainer _host;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected XElement _source;







  protected ZmlComponent(XElement source)
  {
    _source = source;
    var root = _source?.Document?.Root;
  }
  protected ZmlComponent(IZmlContainer host, XElement source) : this(source)
  {
    _host = host;
  }








  public abstract ExtJSClassExpression AsDefine { get; }
  public abstract ObjectBody AsConfig { get; }






  public IZmlContainer Host => _host;
  public string this[string str] => _source.AttrVal(str) ?? null;





  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public XElement XMLSource => _source;






  public TOut As<TOut>() where TOut : class, IZmlComponent => (this is TOut && (this as TOut) != null) ? this as TOut : null;
  public THost Closest<THost>() where THost : class, IZmlContainer
  {
    var ret = Host;
    if (ret != null && ret is THost)
      return ret.As<THost>();
    while (ret != null && !(ret is THost))
      ret = ret.Host;
    return ret?.As<THost>();
  }
  public virtual IList<TOut> Find<TOut>(Func<TOut, bool> predicate) where TOut : class, IZmlComponent
  {
    var ret = new List<TOut>();
    if (As<TOut>() != null && predicate(As<TOut>())) ret.Add(As<TOut>());
    return ret;
  }
  public virtual IList<TOut> Find<TOut>() where TOut : class, IZmlComponent
  {
    var ret = new List<TOut>();
    if (As<TOut>() != null) ret.Add(As<TOut>());
    return ret;
  }







  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public ZmlApplication App => Closest<ZmlApplication>();








  public bool? parseBoolAttr(string xname) => (this[xname] != null && (this[xname].ToLower() == "true" || this[xname].ToLower() == "false")) ? bool.Parse(this[xname].ToLower()) as bool? : null;
  public int? parseIntAttr(string xname) => (this[xname] != null) ? int.Parse(this[xname]) as int? : null;
  public double? parseDoubleAttr(string xname) => (this[xname] != null) ? double.Parse(this[xname].Replace(".", ",")) as double? : null;
  public KeyValue PROP(string key, object val) => val != null ? KV(key, val) : null;
  public KeyValue KVNULL(string key, object val) => val != null ? KV(key, val) : KV(key, NULL);







  /// <summary>
  /// JS-свойство, связывающее имя метода-обработчика события 
  /// с методом запуска исполняемого действия, ссылка на которое содержится в глобальном менеджере APP.ACTION.
  /// </summary>
  /// <param name="handler">Имя метода-обработчика события (метод должен быть определен в ExtJS-классе элемента).</param>
  /// <param name="eventAttr">Идентификатор исполняемого действия, определенного в разметке модуля.</param>
  /// <returns></returns>
  public KeyValue OnEvent(string handler, string eventAttr) => this[eventAttr].IsUseful() ? KV(handler, FN(App.jsrv.Action(this[eventAttr]).Call())) : null;

}
