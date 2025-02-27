using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;


namespace ZMLProject.Models.Data;

/// <summary>
/// Модель для источника данных.
/// </summary>
public class SourceModel : ZmlComponent
{
  protected BaseSource _src;





  public SourceModel(BaseSource source) : base(null) => _src = source;








  public override ExtJSClassExpression AsDefine => Source?.App.jsrv.Ext_define(Classname, OBJ(
    KV("extend", Extend),
    PROP("idProperty", _src.Key),
    KV("fields", ARR_Fields)
    ), Descr);
  public override ObjectBody AsConfig => null;









  public BaseSource Source => _src;

  public string Descr => Source?.App.bsrv.SourceModel(this).Descr;

  public string Classname => Source?.App.bsrv.SourceModel(this).Classname;

  public string Extend => Source?.App.bsrv.SourceModel(this).Extend;










  private ArrayDefinition ARR_Fields => 
    Source?.DataItems.Count() > 0 
    ? 
    Arr(false, true, Source?.DataItems.FirstOrDefault().AsFields) 
    : 
    Arr(false, true, Source?.Fields.Select(s => s.AsConfig));
}
