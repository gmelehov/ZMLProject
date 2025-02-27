using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;


namespace ZMLProject.Models;

/// <summary>
/// Шаблон элемента интерфейса.
/// </summary>
public class Template : ZmlComponent
{
  protected Dictionary<string, string> _templDict;




  public Template(IZmlContainer host, XElement source) : base(host, source)
  {
    _templDict = source.Attributes().Where(w => w.Name.ToString() != "id").ToDictionary(k => k.Name.ToString(), d => d.Value);
  }








  public override ExtJSClassExpression AsDefine => null;
  public override ObjectBody AsConfig => null;
  public ArrayDefinition AsTemplateStrings => Arr(Utils.CompileItemTemplateAsStrings(Utils.ItemTemplatesDict?[Id], DataDict)?.Select(t => Str(t)));
  public AbstractExpression AsExtXTemplate => Utils.GetNewExtTemplate(Id, DataDict);








  public string Id => this["id"];
  public string ItemSelector => this["itemSelector"];
  public Dictionary<string, string> DataDict => _templDict;

}
