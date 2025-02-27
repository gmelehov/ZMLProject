using DiffCode.JSGenerator;
using System.Xml.Linq;
using ZMLProject.Models.Forms;
using System.ComponentModel.DataAnnotations;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "очистить значение поля ввода".
  /// </summary>
  public class ClearInput : BaseCommand
  {
    public ClearInput(IZmlContainer host, XElement source) : base(host, source)
    {

    }






    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs =>
      GetGetFieldType() is BaseFormField && !(GetGetFieldType() is BaseCombo) ? App.jsrv.Input(Id).Reset()
      :
      GetGetFieldType() is BaseCombo ? App.jsrv.Combo(Id).Reset()
      :
      null
      ;







    /// <summary>
    /// Идентификатор поля ввода/комбобокса.
    /// </summary>
    [Required]
    public string Id => this["id"];







    /// <summary>
    /// Возвращает экземпляр поля ввода с идентификатором, равным значению свойства <see cref="Id"/>.
    /// </summary>
    /// <returns></returns>
    private BaseFormField GetGetFieldType()
    {
      if (App != null)
      {
        if (App.HasInput(Id)) return App.GetInput(Id);
        if (App.HasCombo(Id)) return App.GetCombo(Id);
        return null;
      }
      else
      {
        return null;
      }
    }
  }
}
