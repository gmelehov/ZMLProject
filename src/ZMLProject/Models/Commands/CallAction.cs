using DiffCode.JSGenerator;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;





namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "запуск действия".
  /// </summary>
  public class CallAction : BaseCommand
  {
    public CallAction(ZmlContainer host, XElement source) : base(host, source) { }






    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs =>
      GetAction()?.Run == "local" || GetAction()?.Run == "rest"
      ?
      App.jsrv.Action(Id).Call()
      :
      App.jsrv.Ext_GetSoreAndLoad(GetAction()?.Classname)
      ;






    /// <summary>
    /// Идентификатор действия.
    /// </summary>
    [Required]
    public string Id => this["id"];





    /// <summary>
    /// Возвращает экземпляр действия с идентификатором, равным значению свойства <see cref="Id"/>.
    /// </summary>
    /// <returns></returns>
    private BaseAction GetAction() => Closest<ZmlApplication>()?.GetAction(Id);

  }
}
