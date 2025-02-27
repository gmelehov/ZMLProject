using System.Xml.Linq;
using static DiffCode.JSGenerator.Generator;
using DiffCode.JSGenerator;
using DiffCode.CommonExtensions;
using System.Diagnostics;






namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Действие, выполняемое на удаленном сервере.
  /// </summary>
  public class RestAction : BaseAction
  {
    public RestAction(ZmlApplication host, XElement source) : base(host, source)
    {
      _actionProxy = new RestProxy(this);
    }










    /// <summary>
    /// Формирует Javascript-выражение, создающее экземпляр ExtJS-класса ZML.data.Action.
    /// </summary>
    public override AbstractExpression AsCreate => 
      Descr.IsUseful() ?
      _app.jsrv.Ext_create(Classname, OBJ(
        KV("itemId", Id),
        KV("AppNAME", _app.Name),
        KV("runAt", Run),
        PROP("confirmText", ConfirmText),
        KV_UrlPath,
        KV_Method,
        KV_Paramlinkers,
        App.bsrv.RestAction(this).OnAfterRequest,
        OnAccepted,
        OnRejected
        ), Descr)
      :
      _app.jsrv.Ext_create(Classname, OBJ(
        KV("itemId", Id),
        KV("AppNAME", _app.Name),
        KV("runAt", Run),
        PROP("confirmText", ConfirmText),
        KV_UrlPath,
        KV_Method,
        KV_Paramlinkers,
        App.bsrv.RestAction(this).OnAfterRequest,
        OnAccepted,
        OnRejected
        ))
      ;











    /// <summary>
    /// 
    /// </summary>
    public string BaseUrl => this["baseUrl"];

    /// <summary>
    /// 
    /// </summary>
    public string Method => this["method"];

    /// <summary>
    /// 
    /// </summary>
    private string UrlPath => $"{BaseUrl?.TrimEnd('/')}/{Name?.TrimStart('/')}";


    public string AfterRequestParamName => App.bsrv.RestAction(this).AfterRequestParamName;









    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_UrlPath => PROP("url", UrlPath);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Method => PROP("method", Method);

  }
}
