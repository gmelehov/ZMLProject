using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models
{
  /// <summary>
  /// Вложенное меню.
  /// </summary>
  public class SubMenu : ZmlContainer
  {
    public SubMenu(IZmlContainer host, XElement source) : base(host, source)
    {
      source?.Elements().ToList().ForEach(s =>
      {
        var name = s.Name.LocalName;
        ZmlComponent el;
        switch (name)
        {
          case "menu.item": el = new MenuItem(this, s); _items.Add(el); break;
          case "sub.menu": el = new SubMenu(this, s); _items.Add(el); break;

          default: break;
        };
      });
    }











    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(
      KV_Id,
      KV_Text,
      KV_IconCls,
      KV_Cls,
      KV_Menu
      );












    /// <summary>
    /// Текст вложенного меню.
    /// </summary>
    public string Text => this["text"];
    
    /// <summary>
    /// Идентификатор вложенного меню.
    /// </summary>
    public string Id => this["id"];
    
    /// <summary>
    /// Иконка, выводимая слева от текста вложенного меню.
    /// </summary>
    public string Icon => this["icon"];
    
    /// <summary>
    /// Цвет иконки.
    /// </summary>
    public string Color => this["color"];

    /// <summary>
    /// 
    /// </summary>
    public string Cls => App.bsrv.SubMenu(this).Cls;

    /// <summary>
    /// 
    /// </summary>
    public string IconCls => App.bsrv.SubMenu(this).IconCls;
















    public KeyValue KV_Id => KV("itemId", Id);
    public KeyValue KV_Text => KV("text", Text);
    public KeyValue KV_IconCls => KV("iconCls", IconCls);
    public KeyValue KV_Cls => KV("cls", Cls);
    public KeyValue KV_Menu => KV("menu", new NewExpr(App.jsrv.Ext_CreateMenuFromConfig(MenuItems)));







    private ObjectBody MenuItems => OBJ(KV("items", Arr(_items.Select(s => s.AsConfig))));
  }
}
