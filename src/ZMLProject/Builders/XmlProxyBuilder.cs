namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class XmlProxyBuilder : Builder<XmlProxy>
  {
    public XmlProxyBuilder(XmlProxy component) : base(component) { }






    public string Type => "ajax";

    public string ReaderType => "xml";

    public string WriterType => "xml";

  }
}
