using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Параметр.
  /// </summary>
  public class Param : ZmlComponent
  {
    public Param(IZmlContainer host, XElement source) : base(host, source)
    {

    }






    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig { get; }
    

    /// <summary>
    /// JavaScript-выражение для объекта-конфигуратора, используемого при создании нового экземпляра параметра.
    /// </summary>
    public ObjectBody ConfigObject => OBJ(false, false,
      PROP("scopeId", Id),
      PROP("scopeType", Scope),
      PROP("field", Field),
      PROP("as", As),
      PROP("record", Record),
      PROP("aspFrom", AspFrom),
      KV("AppNAME", App.Name),
      Default != null ? KV("default", Str(Default)) : null
      );
    
    /// <summary>
    /// JavaScript-выражение для создания нового экземпляра параметра.
    /// </summary>
    public AbstractExpression AsExprs => App.jsrv.Param_New(this);










    /// <summary>
    /// 
    /// </summary>
    public string Id => this["id"];
    
    /// <summary>
    /// 
    /// </summary>
    public string Scope => this["scope"];
    
    /// <summary>
    /// 
    /// </summary>
    public string Field => this["field"];
    
    /// <summary>
    /// 
    /// </summary>
    public string As => this["as"];
    
    /// <summary>
    /// 
    /// </summary>
    public string Default => this["default"];
    
    /// <summary>
    /// 
    /// </summary>
    public int? Record => parseIntAttr("record");

    /// <summary>
    /// 
    /// </summary>
    public string AspFrom => this["asp-from"];

    /// <summary>
    /// 
    /// </summary>
    public string Classname => App.bsrv.Param(this).Classname;

  }
}
