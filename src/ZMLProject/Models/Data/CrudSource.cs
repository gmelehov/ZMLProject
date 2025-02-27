using System.Xml.Linq;






namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Базовое хранилище данных с поддержкой CRUD-операций.
  /// </summary>
  public abstract class CrudSource : BaseSource, IHasCrud
  {
    protected CrudSource(ZmlApplication host, XElement source) : base(host, source) { }







    /// <summary>
    /// Базовый URL для методов, используемых в этом хранилище данных.
    /// </summary>
    public string BaseUrl => this["baseUrl"];

    /// <summary>
    /// Признак поддержки CRUD-операций.
    /// </summary>
    public bool Crud => parseBoolAttr("crud") ?? false;
  }
}
