using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Dockable
{
  /// <summary>
  /// Вертикальный разделитель элементов тулбара.
  /// </summary>
  public class Separator : BaseToolbarItem
  {
    public Separator(Toolbar host, XElement source) : base(host, source) { }







    public override ObjectBody AsConfig => OBJ(false,
      KV_XType,
      KV_Margin,
      KV_Padding,
      KV_Width,
      KV_Height
      );








    public override string XType => App.bsrv.Separator(this).XType;
  }
}
