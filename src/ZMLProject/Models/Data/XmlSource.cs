using DiffCode.CommonExtensions;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;

using System.Xml.Linq;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// 
  /// </summary>
  public class XmlSource : CrudSource
  {
    public XmlSource(ZmlApplication host, XElement source) : base(host, source)
    {
      _sourceProxy = new XmlProxy(this);
    }










    public override ExtJSClassExpression AsDefine =>
      Descr.IsUseful() ?
      _app.jsrv.Ext_define(Classname, OBJ(
        KV_Extend,
        KV_Model,
        KV_Id,
        KV_SourceId,
        KV_ObjectType,
        KV_Key,
        KV_TREE,
        KV_AutoLoad,
        KV_AutoDestroy,
        KV_AutoSync,
        KV_PageSize,
        KV_GroupField,
        ColumnsProperty,
        Params.Count() > 0 ? KV("paramLinkers", Arr(false, true, Params.Select(s => s.AsExprs))) : null,
        KV_Proxy,
        KV_OnAfterLoad,
        KV_OnAfterWrite
        ), Descr)
      :
      _app.jsrv.Ext_define(Classname, OBJ(
        KV_Extend,
        KV_Model,
        KV_Id,
        KV_SourceId,
        KV_ObjectType,
        KV_Key,
        KV_TREE,
        KV_AutoLoad,
        KV_AutoDestroy,
        KV_AutoSync,
        KV_PageSize,
        KV_GroupField,
        ColumnsProperty,
        Params.Count() > 0 ? KV("paramLinkers", Arr(false, true, Params.Select(s => s.AsExprs))) : null,
        KV_Proxy,
        KV_OnAfterLoad,
        KV_OnAfterWrite
        ))
      ;












    public override string Extend => App.bsrv.XmlSource(this).Extend;

    /// <summary>
    /// 
    /// </summary>
    public string Root => this["root"];
    
    /// <summary>
    /// 
    /// </summary>
    public string Record => this["record"];

  }
}
