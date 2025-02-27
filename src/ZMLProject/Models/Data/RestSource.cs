using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;







namespace ZMLProject.Models.Data
{
  /// <summary>
  /// 
  /// </summary>
  public class RestSource : CrudSource
  {
    public RestSource(ZmlApplication host, XElement source) : base(host, source)
    {
      _sourceProxy = new RestProxy(this);
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











    public override string Extend => App.bsrv.RestSource(this).Extend;

  }
}
