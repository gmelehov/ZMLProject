using DiffCode.JSGenerator;

using System.Xml.Linq;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "удалить значение из целевого свойства элемента".
  /// </summary>
  public class ValueRemover : BasePropertyTrigger
  {
    public ValueRemover(ZmlContainer host, XElement source) : base(host, source) { }






    /// <summary>
    /// 
    /// </summary>
    public override AbstractExpression AsExprs
    {
      get
      {
        if (App.jsrv.Value_Remover.ContainsKey(TargetType))
        {
          return App.jsrv.Value_Remover[TargetType].ContainsKey(TargetProp)
              ? App.jsrv.Value_Remover[TargetType][TargetProp](this)
              : null;
        }
        else
        {
          return null;
        }
      }
    }
  }
}
