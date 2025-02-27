using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Charts
{
  /// <summary>
  /// 
  /// </summary>
  public class Axis : ZmlComponent
  {
    public Axis(Chart host, XElement source) : base(host, source)
    {

    }





    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(
      KV_Type,
      KV_Position,
      KV_Title,
      KV_Grid,
      KV_Minimum,
      KV_Maximum,
      KV_Fields
      );







    public string Type => this["type"];
    public string Position => this["position"];
    public string Title => this["title"];
    public bool Grid => parseBoolAttr("grid") ?? true;
    public IEnumerable<string> Fields => this["fields"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public double? Minimum => (this["min"] != null) ? double.Parse(this["min"].Replace(".", ",")) as double? : null;
    public double? Maximum => (this["max"] != null) ? double.Parse(this["max"].Replace(".", ",")) as double? : null;



    public KeyValue KV_Type => KV("type", Type);
    public KeyValue KV_Position => KV("position", Position);
    public KeyValue KV_Title => PROP("title", Title);
    public KeyValue KV_Grid => PROP("grid", Grid);
    public KeyValue KV_Minimum => PROP("minimum", Minimum);
    public KeyValue KV_Maximum => PROP("maximum", Maximum);
    public KeyValue KV_Fields => KV("fields", Arr(false, false, Fields.Select(s => Str(s))));

  }
}
