using DiffCode.JSGenerator;






namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class LoaderBuilder : Builder<Loader>
  {
    public LoaderBuilder(Loader component) : base(component) { }





    public string Classname => "ZML.data.Loader";








    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnBeforeLoad", "onbeforeload")]
    public KeyValue OnBeforeLoad => Self.OnEvent($"OnBeforeLoad", $"onbeforeload");

    /// <summary>
    /// 
    /// </summary>
    [EventDescriptor("OnAfterLoad", "onafterload")]
    public KeyValue OnAfterLoad => Self.OnEvent($"OnAfterLoad", $"onafterload");
  }
}
