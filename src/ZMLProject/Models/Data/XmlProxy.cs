using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Прокси для доступа к XML-данным.
  /// </summary>
  public class XmlProxy : BaseProxy
  {
    public XmlProxy(XmlSource source) : base(source)
    {

    }









    public override ObjectBody AsConfig => OBJ(false, true,
      KV_Type,
      KV_Url,
      KV_Reader,
      KV_Writer
      );








    /// <summary>
    /// Тип прокси.
    /// </summary>
    public override string Type => Source?.App.bsrv.XmlProxy(this).Type;

    /// <summary>
    /// URL-адрес источника XML-данных.
    /// </summary>
    public string Url => Source.As<XmlSource>()?.BaseUrl ?? Action?.As<RestAction>()?.BaseUrl;
    
    /// <summary>
    /// 
    /// </summary>
    public string Root => Source.As<XmlSource>()?.Root;
    
    /// <summary>
    /// 
    /// </summary>
    public string Record => Source.As<XmlSource>()?.Record;

    /// <summary>
    /// Тип прокси-reader'а.
    /// </summary>
    public string ReaderType => Source?.App.bsrv.XmlProxy(this).ReaderType;

    /// <summary>
    /// Тип прокси-writer'а.
    /// </summary>
    public string WriterType => Source?.App.bsrv.XmlProxy(this).WriterType;









    public override KeyValue KV_Type => KV("type", Type);
    
    public KeyValue KV_Url => PROP("url", Url);

    public KeyValue KV_Root => KVNULL("root", Root);
    
    public KeyValue KV_Record => KV("record", Record);

    public KeyValue KV_Reader => KV("reader", GetReaderConfig());
    
    public KeyValue KV_Writer => KV("writer", GetWriterConfig());





    /// <summary>
    /// Возвращает JavaScript-выражение для объекта-конфигуратора reader.
    /// </summary>
    /// <returns></returns>
    private ObjectBody GetReaderConfig() => OBJ(false, false, KV("type", ReaderType), KV_Root, KV_Record);

    /// <summary>
    /// Возвращает JavaScript-выражение для объекта-конфигуратора writer.
    /// </summary>
    /// <returns></returns>
    private ObjectBody GetWriterConfig() => OBJ(false, false, KV("type", WriterType), KV_Record);
  }
}
