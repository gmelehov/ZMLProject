using DiffCode.JSGenerator;

using System.Xml.Linq;





namespace ZMLProject.Models.Valuables
{
  /// <summary>
  /// Абстрактный элемент, возвращающий значение.
  /// </summary>
  public abstract class BaseValuable : ZmlComponent
  {
    protected BaseValuable(IZmlContainer host, XElement source) : base(host, source)
    {

    }






    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => null;
    public abstract AbstractExpression AsExprs { get; }

  }
}
