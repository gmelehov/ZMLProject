using DiffCode.JSGenerator;

using System.Diagnostics;






namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Абстрактный прокси для хранилища данных.
  /// </summary>
  public abstract class BaseProxy : ZmlComponent
  {
    protected BaseSource _src;
    protected RestAction _action;






    protected BaseProxy(BaseSource source) : base(null) => _src = source;
    protected BaseProxy(RestAction action) : base(null) => _action = action;









    public override ExtJSClassExpression AsDefine => null;









    public BaseSource Source => _src;

    public BaseAction Action => _action;

    public abstract string Type { get; }






    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public abstract KeyValue KV_Type { get; }

  }
}
