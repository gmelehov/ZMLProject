using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Plugins
{
  /// <summary>
  /// Линкер, связывающий внешнее состояние элемента 
  /// с тем или иным событием его внешнего контекста.
  /// </summary>
  public class ContextBinder : ZmlComponent, IContextBinder
  {
    public ContextBinder(IZmlContainer host, XElement source) : base(host, source)
    {

    }








    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, false, KV_For, KV_Scope, KV_ItemId, KV_If, KV_NeedSelection);








    /// <summary>
    /// Идентификатор подключаемого элемента.
    /// Для случаев, когда этот линкер предназначен не для контекстного элемента (в котором он определен в разметке),
    /// а для другого, имеющего этот идентификатор. При этом настройка и добавление управляющего плагина 
    /// к такому элементу выполняются динамически, при инициализации контекстного элемента.
    /// </summary>
    public string For => this["for"];

    /// <summary>
    /// Тип внешнего контекста, управляющего состоянием подключаемого элемента.
    /// Возможные значения см. в определении XSD-типа ContextScopes.
    /// </summary>
    public string Scope => this["scope"];

    /// <summary>
    /// Идентификатор внешнего контекста, управляющего состоянием подключаемого элемента.
    /// В случае, если указан атрибут scope и подключаемый элемент иерархически вложен
    /// в свой внешний контекст, идентификатор можно не указывать.
    /// </summary>
    public string Id => this["id"];

    /// <summary>
    /// Условие, которому должен соответствовать внешний контекст для активации линкера.
    /// Возможные значения см. в определении XSD-типа EnablingContexts.
    /// </summary>
    public string If => this["if"];

    /// <summary>
    /// 
    /// </summary>
    public string HostEvent
    {
      get
      {
        switch (this["if"])
        {
          case "has-selection": return Scope == "Combo" ? "change" : "selectionchange";
          case "is-valid": return "validitychange";
          case "has-changed": return "change";


          default: return this["on"];
        };
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string NeedSelection => this["needSelection"];













    public KeyValue KV_For => PROP("for", For);
    
    
    public KeyValue KV_Scope => PROP("hostType", Scope);
    
    
    public KeyValue KV_ItemId => PROP("hostId", Id);
    
    
    public KeyValue KV_If => PROP("hostEvent", HostEvent);

    /// <summary>
    /// Определение Javascript-свойства needSelection для подключаемого элемента, 
    /// XML-разметка которого содержит атрибут needSelection.
    /// </summary>
    public KeyValue KV_NeedSelection => PROP("needSelection", NeedSelection);
    //public KeyValue KV_NeedSelection => (Host is IHasContextBinders) ? PROP("needSelection", Host.As<IHasContextBinders>()?["needSelection"]) : PROP("needSelection", NeedSelection);
  }
}
