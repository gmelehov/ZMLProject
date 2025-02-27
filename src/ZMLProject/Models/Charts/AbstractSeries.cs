using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Charts
{
  /// <summary>
  /// Абстрактный Series.
  /// </summary>
  public abstract class AbstractSeries : ZmlComponent
  {
    protected AbstractSeries(Chart host, XElement source) : base(host, source)
    {

    }






    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(
      KV_Type,
      KV_Highlight,
      KV_ShowInLegend,
      KV_Title,
      KV_Label
      )
      +
      SpecificKeyValues?.Props
      ;
    protected abstract ObjectBody SpecificKeyValues { get; }






    public abstract string Type { get; }
    public virtual bool? HighLight => parseBoolAttr("highlight");
    public virtual string Label => this["label"];
    public virtual bool? ShowInLegend => parseBoolAttr("showInLegend");
    public virtual string Title => this["title"];



    public KeyValue KV_Type => KV("type", Type);
    public virtual KeyValue KV_Highlight => PROP("highlight", HighLight);
    public KeyValue KV_ShowInLegend => PROP("showInLegend", ShowInLegend);
    public KeyValue KV_Title => PROP("title", Title);
    public abstract KeyValue KV_Label { get; }

  }
}
