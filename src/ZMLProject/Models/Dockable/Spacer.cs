using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;






namespace ZMLProject.Models.Dockable
{
  /// <summary>
  /// Горизонтальный разделитель элементов тулбара.
  /// </summary>
  public class Spacer : BaseToolbarItem
  {
    public Spacer(Toolbar host, XElement source) : base(host, source) { }







    public override ObjectBody AsConfig => OBJ(false, false,
      KV_XType,
      KV_Flex,
      KV_Margin,
      KV_Padding,
      KV_Width,
      KV_Height
      );









    /// <summary>
    /// 
    /// </summary>
    public override string XType => App.bsrv.Spacer(this).XType;

    /// <summary>
    /// 
    /// </summary>
    public double? Flex => parseDoubleAttr("flex");









    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Flex => PROP("flex", Flex);
  }
}
