using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;





namespace ZMLProject.Models
{
  /// <summary>
  /// Элемент меню.
  /// </summary>
  public class MenuItem : ZmlComponent
  {
    public MenuItem(IZmlContainer host, XElement source) : base(host, source) { }






    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, false,
      KV_Id,
      KV_Text,
      KV_IconCls,
      KV_Cls,
      KV_Disabled,
      KV_Hidden,
      App.bsrv.MenuItem(this).OnClick
      );










    /// <summary>
    /// Идентификатор элемента меню.
    /// </summary>
    public string Id => this["id"];

    /// <summary>
    /// Текст элемента меню.
    /// </summary>
    public string Text => this["text"];

    /// <summary>
    /// CSS-стиль иконки, выводимой слева от текста элемента меню.
    /// </summary>
    public string IconCls => App.bsrv.MenuItem(this).IconCls;

    /// <summary>
    /// CSS-стиль элемента меню.
    /// </summary>
    public string Cls => App.bsrv.MenuItem(this).Cls;

    /// <summary>
    /// Признак отключенного элемента меню.
    /// </summary>
    public bool? Disabled => parseBoolAttr("disabled");

    /// <summary>
    /// Признак скрытого элемента меню.
    /// </summary>
    public bool? Hidden => parseBoolAttr("hidden");














    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Id => KV("itemId", Id);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Text => KV("text", Text);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_IconCls => KV("iconCls", IconCls);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Cls => KV("cls", Cls);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Disabled => PROP("disabled", Disabled);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private KeyValue KV_Hidden => PROP("hidden", Hidden);

  }
}
