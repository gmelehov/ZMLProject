using System.Xml.Linq;

using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;






namespace ZMLProject.Models
{
  /// <summary>
  /// Вертикальный/горизонтальный разделитель панелей.
  /// </summary>
  public class Splitter : ZmlComponent
  {
    public Splitter(ZmlContainer host, XElement source) : base(host, source) { }







    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, false,
      PROP("xtype", XType),
      PROP("collapseTarget", CollapseTarget),
      PROP("size", Size),
      PROP("overCls", OverCls)
      );










    /// <summary>
    /// 
    /// </summary>
    public string XType => App.bsrv.Splitter(this).XType;

    /// <summary>
    /// 
    /// </summary>
    public string CollapseTarget => App.bsrv.Splitter(this).CollapseTarget;

    /// <summary>
    /// 
    /// </summary>
    public int? Size => parseIntAttr("size");

    /// <summary>
    /// 
    /// </summary>
    public string OverCls => App.bsrv.Splitter(this).OverCls;

  }
}
