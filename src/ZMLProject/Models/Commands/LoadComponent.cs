using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;





namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "загрузить содержимое серверного компонента".
  /// </summary>
  public class LoadComponent : BaseCommand 
  {
    public LoadComponent(IZmlContainer host, XElement source) : base(host, source) { }





    public override AbstractExpression AsExprs => Id.IsUseful() ? App.jsrv.LocateComponent(Id).AddExpr(CALL("LoadContent", Args())) : App.jsrv.LocateComponent(Id).AddExpr(CALL("LoadAll", Args()));





    public string Id => this["id"];

  }
}
