using DiffCode.JSGenerator;
using System.Xml.Linq;
using ZMLProject.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;





namespace ZMLProject.Models.Valuables
{
  /// <summary>
  /// Значение свойства элемента.
  /// </summary>
  public class GetProp : BaseValuable
  {
    public GetProp(IZmlContainer host, XElement source) : base(host, source)
    {

    }







    /// <summary>
    /// 
    /// </summary>
    public override AbstractExpression AsExprs
    {
      get
      {
        if (App.jsrv.Prop_Getter.ContainsKey(Scope))
        {
          return App.jsrv.Prop_Getter[Scope].ContainsKey(Name)
              ? App.jsrv.Prop_Getter[Scope][Name](this)
              : App.jsrv.Prop_Getter[Scope].ContainsKey(ElProps.Value)
              ? App.jsrv.Prop_Getter[Scope][ElProps.Value](this)
              : null;
        }
        else
        {
          return null;
        }
      }
    }









    /// <summary>
    /// Наименование свойства элемента.
    /// </summary>
    [Required]
    public string Name => this["name"];

    /// <summary>
    /// Тип элемента.
    /// </summary>
    [Required]
    public string Scope => this["scope"];

    /// <summary>
    /// Идентификатор элемента.
    /// </summary>
    [Required]
    public string ScopeId => this["scopeId"];

    /// <summary>
    /// Индекс записи, из которой извлекается необходимое свойство.
    /// </summary>
    [DefaultValue(0)]
    public int Record => parseIntAttr("record") ?? 0;

  }
}
