using System.Xml.Linq;
using static DiffCode.JSGenerator.Generator;
using DiffCode.JSGenerator;
using DiffCode.CommonExtensions;
using ZMLProject.Enums;







namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Загрузчик содержимого.
  /// </summary>
  [ManagedProps(PropAccess.Getter, ElProps.Url, ElProps.Mode)]
  [ManagedProps(PropAccess.Setter, ElProps.Url)]
  [ManagedProps(PropAccess.Caller, "Load")]
  public class Loader : ZmlContainer
  {
    public Loader(ZmlApplication host, XElement source) : base(host, source)
    {
      var elems = source.Elements().ToList();
      elems.ForEach(b =>
      {
        IZmlComponent elem = null;
        switch (b.Name.LocalName)
        {
          case "Prm": elem = new Param(this, b); break;


          default: break;
        };
        if (elem != null) _items.Add(elem);
      });
    }











    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(
      KV("itemId", Id),
      KV("AppNAME", App.Name),
      KV("url", Url),
      KV("renderMode", Mode),
      PROP("replaceContent", ReplaceContent),
      KV("paramLinkers", Arr(false, true, Params.Select(s => s.AsExprs))),
      App.bsrv.Loader(this).OnBeforeLoad,
      App.bsrv.Loader(this).OnAfterLoad
      );
    public AbstractExpression AsCreate => Descr.IsUseful() ? App.jsrv.Ext_create(Classname, AsConfig, Descr) : App.jsrv.Ext_create(Classname, AsConfig);
    public AbstractExpression AsRegister => App.jsrv.Loader(Id).MGR_Register(AsCreate);










    /// <summary>
    /// 
    /// </summary>
    public string Id => this["id"];

    /// <summary>
    /// 
    /// </summary>
    public string Classname => App.bsrv.Loader(this).Classname;

    /// <summary>
    /// 
    /// </summary>
    public string Descr => this["descr"];

    /// <summary>
    /// 
    /// </summary>
    public string Url => this["url"];

    /// <summary>
    /// 
    /// </summary>
    public string Mode => this["mode"];

    /// <summary>
    /// 
    /// </summary>
    public bool? ReplaceContent => parseBoolAttr("replaceContent");









    protected IEnumerable<Param> Params => _items.OfType<Param>();

  }
}
