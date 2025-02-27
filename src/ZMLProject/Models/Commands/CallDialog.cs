using DiffCode.JSGenerator;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "показать диалоговое окно".
  /// </summary>
  public class CallDialog : BaseCommand
  {
    public CallDialog(IZmlContainer host, XElement source) : base(host, source)
    {

    }










    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs => App.jsrv.Dialog(Id).Show();







    /// <summary>
    /// Идентификатор диалогового окна.
    /// </summary>
    [Required]
    public string Id => this["id"];

  }
}
