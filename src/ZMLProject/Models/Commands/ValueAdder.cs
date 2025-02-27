using DiffCode.JSGenerator;

using System.Xml.Linq;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "добавить значение в целевое свойство элемента".
  /// </summary>
  public class ValueAdder : BasePropertyTrigger
  {
    public ValueAdder(ZmlContainer host, XElement source) : base(host, source) { }






    /// <summary>
    /// 
    /// </summary>
    public override AbstractExpression AsExprs
    {
      get
      {
        if (App.jsrv.Value_Adder.ContainsKey(TargetType))
        {
          return App.jsrv.Value_Adder[TargetType].ContainsKey(TargetProp)
              ? App.jsrv.Value_Adder[TargetType][TargetProp](this)
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
