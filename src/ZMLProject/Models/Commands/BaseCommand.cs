using DiffCode.JSGenerator;

using System.Xml.Linq;





namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Абстрактная исполняемая команда.
  /// </summary>
  public abstract class BaseCommand : ZmlContainer
  {
    protected BaseCommand(IZmlContainer host, XElement source) : base(host, source) { }





    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => null;






    public abstract AbstractExpression AsExprs { get; }

  }
}
