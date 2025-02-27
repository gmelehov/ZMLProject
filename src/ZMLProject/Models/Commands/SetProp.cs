using DiffCode.JSGenerator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;
using ZMLProject.Models.Valuables;
using DiffCode.JSGenerator.Extensions;
using ZMLProject.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда "задать значение свойства целевого элемента".
  /// </summary>
  public class SetProp : BaseCommand
  {
    private GetProp _getter;






    public SetProp(ZmlContainer host, XElement source) : base(host, source) => ProcessGetter();









    private SetProp ProcessGetter()
    {
      if (HasSourceDefined())
      {
        var getterXml = new XElement(XName.Get("get.prop", "ZMLAppSchema"), new XAttribute("name", SourceProp), new XAttribute("scope", SourceType), new XAttribute("scopeId", SourceId));
        if (this._source.HasAttr("record"))
        {
          getterXml.SetAttributeValue("record", this._source.AttrVal("record"));
        };

        this._source.AddToFirst(false, getterXml);
        var getter = new GetProp(this, getterXml);
        this._items.Add(getter);
        this._source.RemoveAttrs(new XName[] { "sourceId", "sourceType", "sourceProp", "record" });
        this._getter = getter;
      };
      return this;
    }









    /// <summary>
    /// 
    /// </summary>
    public override AbstractExpression AsExprs
    {
      get
      {
        if (App.jsrv.Prop_Setter.ContainsKey(TargetType))
        {
          return App.jsrv.Prop_Setter[TargetType].ContainsKey(TargetProp)
              ? App.jsrv.Prop_Setter[TargetType][TargetProp](this)
              : App.jsrv.Prop_Setter[TargetType].ContainsKey(ElProps.Value)
              ? App.jsrv.Prop_Setter[TargetType][ElProps.Value](this)
              : null;
        }
        else
        {
          return null;
        }
      }
    }









    /// <summary>
    /// Идентификатор целевого элемента.
    /// </summary>
    [Required]
    public string TargetId => this["targetId"];

    /// <summary>
    /// Тип (категория) целевого элемента.
    /// </summary>
    [Required]
    public string TargetType => this["targetType"];

    /// <summary>
    /// Свойство/поле в модели данных целевого элемента.
    /// </summary>
    [Required]
    public string TargetProp => this["targetProp"];

    /// <summary>
    /// Идентификатор элемента-источника.
    /// </summary>
    public string SourceId => this["sourceId"];

    /// <summary>
    /// Тип (категория) элемента-источника.
    /// </summary>
    public string SourceType => this["sourceType"];

    /// <summary>
    /// Свойство/поле в модели данных элемента-источника.
    /// </summary>
    public string SourceProp => this["sourceProp"];

    /// <summary>
    /// 
    /// </summary>
    public string As => this["as"];

    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(0)]
    public int Record => parseIntAttr("record") ?? 0;










    private bool HasTargetDefined() => TargetId.IsUseful() && TargetType.IsUseful() && TargetProp.IsUseful();
    private bool HasSourceDefined() => SourceId.IsUseful() && SourceType.IsUseful() && SourceProp.IsUseful();






    public MemberAccess MEMB_Target => App.jsrv.LocateByCategory(TargetType, TargetId);
    public AbstractExpression MEMB_Source => (HasSourceDefined() ? App.jsrv.LocateByCategory(SourceType, SourceId) : _getter?.AsExprs.As<MemberAccess>()) ?? HostResult;




    public AbstractExpression HostResult { get; set; }

  }
}
