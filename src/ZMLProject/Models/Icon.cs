using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models
{
  /// <summary>
  /// Элемент интерфейса:
  /// иконка в столбце таблицы данных.
  /// </summary>
  public class Icon : ZmlComponent
  {
    public Icon(Field host, XElement source) : base(host, source) { }







    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => null;
    public AbstractExpression AsExprs => Color != null ? new NewExpr(Utils.MatIconLinker("ZML", ConfigObj)) as AbstractExpression : new NewExpr(Utils.IconLinker("ZML", ConfigObj)) as AbstractExpression;


    public ObjectBody ConfigObj => OBJ(false,
      KV("value", CaseValue),
      KV("name", Name),
      Color != null ? KV("color", Color) : null,
      Title != null ? KV("title", Title) : null
      );








    public string Name => this["name"];
    public string Color => this["color"];
    public string Title => this["title"];
    public object CaseValue
    {
      get
      {
        switch (this["value"].ToLower())
        {
          case "true": return true;
          case "false": return false;

          default: return this["value"];
        };
      }
    }
  }
}
