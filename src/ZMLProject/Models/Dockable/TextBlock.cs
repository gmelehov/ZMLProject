using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;






namespace ZMLProject.Models.Dockable
{
  /// <summary>
  /// Текстовый элемент тулбара.
  /// </summary>
  public class TextBlock : BaseToolbarItem, IHasXType
  {
    public TextBlock(Toolbar host, XElement source) : base(host, source) { }







    public override ObjectBody AsConfig => OBJ(false,
      KV_XType,
      KV_ItemId,
      KV_Text,
      KV_Align,
      KV_Flex,
      KV_Margin,
      KV_Padding,
      KV_Width,
      KV_Height
      );









    public string Id => this["id"];
    public override string XType => App.bsrv.TextBlock(this).XType;
    public string Text => this["text"];
    public string Align => this["align"];
    public double? Flex => parseDoubleAttr("flex");








    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_ItemId => PROP("itemId", Id);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Text => PROP("text", Text);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Align => PROP("align", Align);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Flex => PROP("flex", Flex);
  }
}
