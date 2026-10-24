using DiffCode.JSGenerator;

using System.Xml.Linq;






namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Абстрактное поле модели данных.
  /// </summary>
  public abstract class BaseField : ZmlContainer
  {
    protected BaseField(IZmlContainer host, XElement source) : base(host, source) { }







    public override ExtJSClassExpression AsDefine => null;







    public virtual string Name => this["name"] ?? this["id"];

    public abstract ObjectBody Column { get; }

  }
}
