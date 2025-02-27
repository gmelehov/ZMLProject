using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;







namespace ZMLProject.Models.Grids
{
  /// <summary>
  /// Контекстное меню.
  /// </summary>
  public class ContextMenu : ZmlContainer
  {
    public ContextMenu(IZmlContainer host, XElement source) : base(host, source)
    {
      source?.Elements().ToList().ForEach(s =>
      {
        var name = s.Name.LocalName;
        IZmlComponent el;
        switch (name)
        {
          case "menu.item": el = new MenuItem(this, s); _items.Add(el); break;
          case "sub.menu": el = new SubMenu(this, s); _items.Add(el); break;

          default: break;
        };
      });
    }







    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => null;









    public string Title => this["title"];
    public int? MaxWidth => parseIntAttr("maxWidth");





    public KeyValue KV_CtxMenuTitle => PROP("CtxMenuTitle", Title);
    public KeyValue KV_CtxMenuItems => KV("CtxMenuItems", Arr(Items?.Select(s => s.AsConfig)));

  }
}
