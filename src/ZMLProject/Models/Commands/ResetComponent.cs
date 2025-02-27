using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;




namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "удалить содержимое серверного компонента".
  /// </summary>
  public class ResetComponent : BaseCommand
  {
    public ResetComponent(IZmlContainer host, XElement source) : base(host, source) { }





    public override AbstractExpression AsExprs => Id.IsUseful() ? App.jsrv.LocateComponent(Id).AddExpr(CALL("RemoveContent", Args())) : App.jsrv.LocateComponent(Id).AddExpr(CALL("ResetAll", Args()));





    public string Id => this["id"];

  }
}
