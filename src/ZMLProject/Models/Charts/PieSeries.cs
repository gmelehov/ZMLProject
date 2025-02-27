using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;




namespace ZMLProject.Models.Charts
{
  /// <summary>
  /// Pie series.
  /// </summary>
  public class PieSeries : AbstractSeries
  {
    public PieSeries(Chart host, XElement source) : base(host, source)
    {

    }






    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_AngleField
      );







    public override string Type => "pie";
    public string AngleField => this["xfield"];
    public IEnumerable<string> LabelFields => this["labelfield"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public string LabelPos => this["labelPos"];




    public KeyValue KV_AngleField => KV("angleField", AngleField);
    public override KeyValue KV_Highlight => KV("highlight", OBJ(false, false, KV("segment", OBJ(false, false, KV("margin", 20)))));
    public override KeyValue KV_Label => KV("label", OBJ(false, false,
      KV("display", LabelPos),
      KV("contrast", true),
      KV("field", Arr(false, false, LabelFields.Select(s => Str(s)))),
      KV("font", "500 11px Fira Sans Condensed")
      ));

  }
}
