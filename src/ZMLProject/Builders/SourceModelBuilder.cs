namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class SourceModelBuilder : Builder<SourceModel>
  {
    public SourceModelBuilder(SourceModel component) : base(component) { }








    public string Descr => $"Модель для источника данных {Self.Source?.Classname}";

    public string Classname => $"ZML.model.{Self.Source?.Id}";

    public string Extend => $"Ext.data.Model";

  }
}
