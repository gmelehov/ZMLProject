using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Charts
{
  /// <summary>
  /// Line series.
  /// </summary>
  public class LineSeries : AbstractSeries
  {
    public LineSeries(Chart host, XElement source) : base(host, source)
    {

    }






    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Axis,
      KV_Fill,
      KV_Smooth,
      KV_ShowMarkers,
      KV_MarkerConfig,
      KV_XField,
      KV_YField
      );








    public override string Type => "line";
    public IEnumerable<string> Axis => this["axis"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public string LabelPos => this["labelPos"];
    public string XField => this["xfield"];
    public IEnumerable<string> YField => this["yfield"].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public bool? Fill => parseBoolAttr("fill");
    public bool? Smooth => parseBoolAttr("smooth");
    public bool? ShowMarkers => parseBoolAttr("showMarkers");




    public KeyValue KV_Axis => KV("axis", Arr(false, false, Axis.Select(s => Str(s))));
    public override KeyValue KV_Highlight => PROP("highlight", HighLight);
    public KeyValue KV_Fill => PROP("fill", Fill);
    public KeyValue KV_Smooth => PROP("smooth", Smooth);
    public KeyValue KV_ShowMarkers => PROP("showMarkers", ShowMarkers);
    public KeyValue KV_MarkerConfig => ShowMarkers == true ? KV("markerConfig", OBJ(false, false,
      KV("type", "circle"),
      KV("radius", 4)
      ))
      : null;
    public override KeyValue KV_Label => LabelPos != null ? KV("label", OBJ(false, false,
      KV("display", LabelPos),
      KV("contrast", true),
      KV("field", Arr(false, false, YField.Select(s => Str(s)))),
      KV("font", "500 11px Fira Sans")
      ))
      : null;
    public KeyValue KV_XField => PROP("xField", XField);
    public KeyValue KV_YField => KV("yField", Arr(false, false, YField.Select(s => Str(s))));


  }
}
