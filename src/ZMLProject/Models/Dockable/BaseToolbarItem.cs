using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;






namespace ZMLProject.Models.Dockable
{
  /// <summary>
  /// Абстрактный дочерний элемент тулбара.
  /// </summary>
  public abstract class BaseToolbarItem : ZmlComponent, IHasBaseRectSizes, IHasXType
  {
    protected BaseToolbarItem(Toolbar host, XElement source) : base(host, source) { }






    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, false,
      KV_XType,
      KV_Margin,
      KV_Padding,
      KV_Width,
      KV_Height
      );









    public abstract string XType { get; }
    public string Margin => this["margin"];
    public string Padding => this["padding"];
    public string Width => this["width"];
    public string Height => this["height"];








    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_XType => KV("xtype", XType);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Margin => PROP("margin", Margin);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Padding => PROP("padding", Padding);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Width => PROP("width", Width);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Height => PROP("height", Height);
  }
}
