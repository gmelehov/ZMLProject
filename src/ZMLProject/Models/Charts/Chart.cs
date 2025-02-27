using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Charts
{
  /// <summary>
  /// График.
  /// </summary>
  public class Chart : ZmlContainer
  {
    public Chart(ZmlContainer host, XElement source) : base(host, source)
    {
      _items.AddRange(source.Elements().Select(el =>
      {
        var name = el.Name.LocalName;
        IZmlComponent ret;
        switch (name)
        {
          case "axis": ret = new Axis(this, el); break;
          case "pie.series": ret = new PieSeries(this, el); break;
          case "bar.series": ret = new BarSeries(this, el); break;
          case "line.series": ret = new LineSeries(this, el); break;
          

          default: ret = null; break;
        };
        return ret;
      })
      .Where(w => w != null));
    }






    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(
      KV_XType,
      KV_Store,
      KV_ItemId,
      KV_AppNAME,
      KV_Cls,
      KV_Animate,
      KV_Shadow,
      KV_Legend,
      KV_Theme,
      KV_Width,
      KV_Height,
      KV_MinWidth,
      KV_MaxWidth,
      KV_MinHeight,
      KV_MaxHeight,
      KV_Flex,
      KV_Region,
      KV_Border,
      KV_Margin,
      KV_Padding,
      KV_InsetPadding,
      KV_Axes,
      KV_Series
      );







    public BaseSource Source => Closest<ZmlApplication>()?.Sources.FirstOrDefault(f => f.Id == Src);







    public string Id => this["id"];
    public string XType => "chart";
    public string Src => this["src"];
    public string StoreId => $"{Src}";
    public bool? Animate => parseBoolAttr("animate");
    public bool? Shadow => parseBoolAttr("shadow");
    public string Legend => this["legend"];
    public string Cls => this["cls"];
    public double? Flex => (this["flex"] != null) ? double.Parse(this["flex"].Replace(".", ",")) as double? : null;
    public string Margin => this["margin"];
    public string Padding => this["padding"];
    public string InsetPadding => this["insetPadding"];
    public string Theme => this["theme"];
    public string Width => this["width"];
    public string MinWidth => this["minWidth"];
    public string MaxWidth => this["maxWidth"];
    public string MinHeight => this["minHeight"];
    public string MaxHeight => this["maxHeight"];
    public string Height => this["height"];
    public string Region => this["region"];
    public int? Border => (this["border"] != null) ? int.Parse(this["border"]) as int? : null;
    public IEnumerable<Axis> Axes => _items.OfType<Axis>();
    public IEnumerable<AbstractSeries> Series => _items.OfType<AbstractSeries>();








    public KeyValue KV_XType => KV("xtype", XType);
    public KeyValue KV_ItemId => PROP("itemId", Id);
    public KeyValue KV_Store => KV("store", StoreId);
    public KeyValue KV_Cls => PROP("cls", Cls);
    public KeyValue KV_Animate => PROP("animate", Animate);
    public KeyValue KV_Shadow => PROP("shadow", Shadow);
    public KeyValue KV_Flex => PROP("flex", Flex);
    public KeyValue KV_Margin => PROP("margin", Margin);
    public KeyValue KV_Padding => PROP("padding", Padding);
    public KeyValue KV_InsetPadding => PROP("insetPadding", InsetPadding);
    public KeyValue KV_Theme => PROP("theme", Theme);
    public KeyValue KV_Width => PROP("width", Width);
    public KeyValue KV_MinWidth => PROP("minWidth", MinWidth);
    public KeyValue KV_MaxWidth => PROP("maxWidth", MaxWidth);
    public KeyValue KV_Height => PROP("height", Height);
    public KeyValue KV_MinHeight => PROP("minHeight", MinHeight);
    public KeyValue KV_MaxHeight => PROP("maxHeight", MaxHeight);
    public KeyValue KV_Region => PROP("region", Region);
    public KeyValue KV_Border => PROP("border", Border);
    public KeyValue KV_Axes => KV("axes", Arr(false, true, Axes.Select(s => s.AsConfig)));
    public KeyValue KV_Series => KV("series", Arr(false, true, Series.Select(s => s.AsConfig)));
    public KeyValue KV_Legend => Legend != null ? KV("legend", OBJ(false, false, KV("position", Legend))) : null;





    public KeyValue KV_AppNAME => KV("AppNAME", App.Name);

  }
}
