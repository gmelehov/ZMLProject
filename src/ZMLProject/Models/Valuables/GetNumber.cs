using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;




namespace ZMLProject.Models.Valuables
{
  /// <summary>
  /// Число.
  /// </summary>
  public class GetNumber : BaseValuable
  {
    public GetNumber(IZmlContainer host, XElement source) : base(host, source)
    {

    }





    public override AbstractExpression AsExprs => Num(Value);



    public double? Value => parseDoubleAttr("value");

  }
}
