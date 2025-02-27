using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Charts
{
  /// <summary>
  /// Bar/Column series.
  /// </summary>
  public class BarSeries : AbstractSeries
  {
    public BarSeries(Chart host, XElement source) : base(host, source)
    {

    }






    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Column,
      KV_Axis,
      KV_Stacked,
      KV_XField,
      KV_YField
      );





    public bool Column => parseBoolAttr("column") ?? false;
    public override string Type => Column == true ? "column" : "bar";
    public IEnumerable<string> Axis => this["axis"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public bool? Stacked => parseBoolAttr("stacked");
    public string LabelPos => this["labelPos"];
    public string XField => this["xfield"];
    public IEnumerable<string> YField => this["yfield"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);




    public KeyValue KV_Column => KV("column", Column);
    public KeyValue KV_Axis => KV("axis", Arr(false, false, Axis.Select(s => Str(s))));
    public override KeyValue KV_Highlight => PROP("highlight", HighLight);
    public KeyValue KV_Stacked => PROP("stacked", Stacked);
    public KeyValue KV_XField => PROP("xField", XField);
    public KeyValue KV_YField => KV("yField", Arr(false, false, YField.Select(s => Str(s))));
    public override KeyValue KV_Label => KV("label", OBJ(false, false, 
      KV("display", LabelPos),
      KV("contrast", true),
      KV("field", Arr(false, false, YField.Select(s => Str(s)))),
      KV("font", "500 11px Fira Sans Condensed")
      ));

  }
}
