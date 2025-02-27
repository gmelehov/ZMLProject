namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class XmlSourceBuilder : Builder<XmlSource>
  {
    public XmlSourceBuilder(XmlSource component) : base(component) { }





    public string Extend => $"ZML.store.Xml";

  }
}
