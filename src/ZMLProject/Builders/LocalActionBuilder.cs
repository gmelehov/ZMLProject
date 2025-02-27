using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class LocalActionBuilder : Builder<LocalAction>
  {
    public LocalActionBuilder(LocalAction component) : base(component) { }








    /// <summary>
    /// 
    /// </summary>
    public KeyValue OnExecute => KV($"__Execute", FN(Self.Commands));
  }
}
