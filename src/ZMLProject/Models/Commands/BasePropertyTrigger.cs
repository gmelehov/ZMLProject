using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;
using ZMLProject.Models.Valuables;
using DiffCode.JSGenerator.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;






namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Абстрактный переключатель значения целевого свойства элемента.
  /// </summary>
  public abstract class BasePropertyTrigger : BaseCommand
  {
    protected GetProp _getter;







    protected BasePropertyTrigger(ZmlContainer host, XElement source) : base(host, source) => ProcessGetter();
    








    protected BasePropertyTrigger ProcessGetter()
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
    /// Значение, добавляемое к целевому свойству элемента / удаляемое из целевого свойства элемента.
    /// </summary>
    public string Value => this["value"];

    /// <summary>
    /// 
    /// </summary>
    [DefaultValue(0)]
    public int Record => parseIntAttr("record") ?? 0;










    private bool HasSourceDefined() => SourceId.IsUseful() && SourceType.IsUseful() && SourceProp.IsUseful();






    public MemberAccess MEMB_Target => App.jsrv.LocateByCategory(TargetType, TargetId);
    public AbstractExpression MEMB_Source => (HasSourceDefined() ? App.jsrv.LocateByCategory(SourceType, SourceId) as AbstractExpression : _getter?.AsExprs.As<MemberAccess>()) as AbstractExpression ?? Str(Value);

  }
}
