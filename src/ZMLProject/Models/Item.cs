using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;




namespace ZMLProject.Models
{
  /// <summary>
  /// Элемент данных.
  /// </summary>
  public class Item : ZmlComponent
  {
    protected Dictionary<string, string> _templDict;




    public Item(IZmlContainer host, XElement source) : base(host, source)
    {
      _templDict = source.Attributes().ToDictionary(k => k.Name.ToString(), d => d.Value);
    }








    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, false, _templDict.Select(d => KV(d.Key, d.Value)));
    public IEnumerable<StringLiteral> AsFields => _templDict.Select(s => Str(s.Key));

  }
}
