using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Valuables
{
  /// <summary>
  /// Текст.
  /// </summary>
  public class GetText : BaseValuable
  {
    public GetText(IZmlContainer host, XElement source) : base(host, source)
    {

    }






    public override AbstractExpression AsExprs => Str(Value);





    public string Value => this["value"];
  }
}
