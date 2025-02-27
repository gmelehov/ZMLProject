using DiffCode.JSGenerator;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;





namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "добавить новую запись".
  /// </summary>
  public class CreateRecord : BaseCommand
  {
    public CreateRecord(IZmlContainer host, XElement source) : base(host, source)
    {
      _items.AddRange(source.Elements(F("Prm")).Select(s => new Param(this, s)).ToList());
    }






    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs => App.jsrv.Ext_StoreManager_SaveNewRecord(Id, Params);






    /// <summary>
    /// Идентификатор хранилища данных, для которого создается новая запись.
    /// </summary>
    [Required]
    public string Id => this["id"];





    /// <summary>
    /// Коллекция дочерних параметров для построения новой записи.
    /// </summary>
    public IEnumerable<Param> Params => _items.OfType<Param>();

  }
}
