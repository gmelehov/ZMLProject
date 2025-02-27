using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class RestActionBuilder : Builder<RestAction>
  {
    public RestActionBuilder(RestAction component) : base(component) { }






    public string AfterRequestParamName => "data";





    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnAfterRequest => KV($"OnAfterRequest", FN(Args(ID(AfterRequestParamName)), Self.Commands));
  }
}
