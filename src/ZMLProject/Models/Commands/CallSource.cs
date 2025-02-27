using DiffCode.JSGenerator;

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;




namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "загрузка источника данных".
  /// </summary>
  public class CallSource : BaseCommand
  {
    public CallSource(ZmlContainer host, XElement source) : base(host, source)
    {

    }






    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs => App.jsrv.Ext_GetSoreAndLoad(Id);







    /// <summary>
    /// Идентификатор источника данных.
    /// </summary>
    [Required]
    public string Id => this["id"];

  }
}
