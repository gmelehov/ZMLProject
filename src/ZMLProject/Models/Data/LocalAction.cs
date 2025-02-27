using System.Xml.Linq;
using static DiffCode.JSGenerator.Generator;
using DiffCode.JSGenerator;
using DiffCode.CommonExtensions;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Локальное действие, логика которого определяется в разметке.
  /// </summary>
  public class LocalAction : BaseAction
  {
    public LocalAction(ZmlApplication host, XElement source) : base(host, source)
    {

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
        KV_Paramlinkers,
        App.bsrv.LocalAction(this).OnExecute,
        OnAccepted,
        OnRejected
        ), Descr)
      :
      _app.jsrv.Ext_create(Classname, OBJ(
        KV("itemId", Id),
        KV("AppNAME", _app.Name),
        KV("runAt", Run),
        PROP("confirmText", ConfirmText),
        KV_Paramlinkers,
        App.bsrv.LocalAction(this).OnExecute,
        OnAccepted,
        OnRejected
        ))
      ;


  }
}
