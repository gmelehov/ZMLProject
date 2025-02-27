using DiffCode.JSGenerator;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "очистить содержимое редактора кода".
  /// </summary>
  public class ClearEditor : BaseCommand
  {
    public ClearEditor(IZmlContainer host, XElement source) : base(host, source)
    {

    }






    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs => App.jsrv.Editor(Id).Clear();







    /// <summary>
    /// Идентификатор редактора кода.
    /// </summary>
    [Required]
    public string Id => this["id"];

  }
}
