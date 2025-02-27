using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Forms;
using DiffCode.CommonExtensions;
using ZMLProject.Services;
using System.Diagnostics;


namespace ZMLProject.Models;

/// <summary>
/// Приложение.
/// </summary>
public class ZmlApplication : ZmlContainer
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public readonly IBuilderService bsrv;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public readonly IJSExpressionsService jsrv;







  public ZmlApplication(IBuilderService builder, IJSExpressionsService jsexpr, XElement source) : base(null, source)
  {

    bsrv = builder;
    jsrv = jsexpr;

    jsrv.SetNewAppInfo(GlobalVar, Name);


    Sources = new List<BaseSource>();
    Actions = new List<BaseAction>();
    Loaders = new List<Loader>();

    var sources = source.Element(F("Sources"));
    var actions = source.Element(F("Actions"));
    var loaders = source.Element(F("Loaders"));


    // Парсим и добавляем хранилища данных
    if (sources != null) Sources.AddRange(sources.Elements(F("Source")).Select(s =>
    {
      switch (s.AttrVal("provider"))
      {
        case "local": return new LocalSource(this, s).As<BaseSource>();
        case "rest": return new RestSource(this, s).As<BaseSource>();
        case "xml": return new XmlSource(this, s).As<BaseSource>();

        default: return null;
      };
    }).Where(w => w != null));


    // Парсим и добавляем исполняемые действия
    if (actions != null) Actions.AddRange(actions.Elements(F("Action")).Select(s => 
    {
      switch (s.AttrVal("run"))
      {
        case "local": return new LocalAction(this, s).As<BaseAction>();
        case "rest": return new RestAction(this, s).As<BaseAction>();

        default: return null;
      };
    }).Where(w => w != null));

    
    // Парсим и добавляем загрузчики
    if(loaders != null)
    {
      Loaders.AddRange(loaders.Elements(F("Loader")).Select(s => new Loader(this, s)).Where(w => w != null));
    };


    // Парсим и добавляем вьюпорт
    ViewPort = new ViewPort(this, source.Element(F("ViewPort")));
  }









  /// <summary>
  /// Записывает результат компиляции приложения в файл.
  /// Путь к файлу извлекается из свойства <see cref="ScriptsTarget"/>.
  /// </summary>
  public void SaveCompiledScript()
  {
    if (ScriptsTarget.IsUseful() && AsAppJSContent.IsUseful())
    {
      File.WriteAllText(ScriptsTarget, AsAppJSContent);
    };
  }








  /// <summary>
  /// 
  /// </summary>
  public string AsAppJSContent =>
    $"" +
    //$"{CompiledScripts}" +
    //$"\r\n\r\n\r\n\r\n\r\n\r\n" +
    $"{EXPRS(new AbstractExpression[] { }.Concat(Sources.SelectMany(s => s.RelatedClasses)).Append(NEWLINES(3))).Formatted}" +
    //$"{EXPRS(new AbstractExpression[] { }.Concat(Actions.Select(s => s.AsDefine).Where(w => w != null)).Append(NEWLINES(3))).Formatted}" +
    $"{EXPRS(new AbstractExpression[] { AsApplication }).Formatted}";
  
  /// <summary>
  /// 
  /// </summary>
  public MemberAccess AsApplication => jsrv.Ext_application(OBJ(
    KV_Name,
    KV_Launch
    ));


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override ExtJSClassExpression AsDefine => null;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public override ObjectBody AsConfig => null;








  /// <summary>
  /// Имя по умолчанию для глобальной переменной, хранящей состояние приложения.
  /// </summary>
  public string GlobalVar => bsrv.ZmlApplication(this).GlobalVar;

  /// <summary>
  /// Имя экземпляра приложения.
  /// </summary>
  public string Name => bsrv.ZmlApplication(this).Name;

  /// <summary>
  /// Путь к файлу, в который будет записан результат компиляции.
  /// </summary>
  public string ScriptsTarget => this["target"];
  
  /// <summary>
  /// Идентификатор (имя) действия, выполняемого при загрузке приложения.
  /// </summary>
  public string Onload => this["onload"];










  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private KeyValue KV_Name => KV(1, "name", Name);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private KeyValue KV_Launch => KV(2, "launch", FN(LaunchContent));















  public List<BaseSource> Sources { get; set; }
  public List<BaseAction> Actions { get; set; }
  public List<Loader> Loaders { get; set; }
  public ViewPort ViewPort { get; set; }







  private ExpressionsSet LaunchContent => EXPRS(
    storesCreation.Append(NEWLINES(3)),
    actionsCreation.Append(NEWLINES(3)),
    loadersCreation.Append(NEWLINES(3)),
    dialogsAssign.Append(NEWLINES(3)),
    viewportCreation.Append(NEWLINES(3)),
    new AbstractExpression[]
    {
      ViewPort.ONResize,
    }
    );




  private IEnumerable<(string, string)> stores => Sources.Select(s => (s.Classname, s.Id));









  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private IEnumerable<AbstractExpression> viewportCreation => new AbstractExpression[]
  {
    ViewPort != null ? ASSIGN(jsrv.APP_VIEW(), ViewPort.AsCreate).AddComment("Основное представление (страница) приложения") : null
  };


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private IEnumerable<AbstractExpression> storesCreation => stores.Select(ss => ASSIGN(jsrv.LocateSource(ss.Item2), jsrv.Ext_create(ss.Item1, OBJ(false, false, KV("storeId", ss.Item2)))));


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private IEnumerable<AbstractExpression> dialogsAssign => Dialogs.Select(s => jsrv.Ext_widget(s.AsConfig));


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private IEnumerable<AbstractExpression> actionsCreation => Actions.Select(s => s.AsCreate);


  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private IEnumerable<AbstractExpression> loadersCreation => Loaders.Select(s => s.AsRegister);











  public IEnumerable<BaseFormField> Inputs => ViewPort.Find<BaseFormField>().Except(Combos).ToList();
  public IEnumerable<BaseCombo> Combos => ViewPort.Find<BaseCombo>();
  public IEnumerable<Dialog> Dialogs => ViewPort.Find<Dialog>();





  public bool HasInput(string id) => Inputs.Any(a => a.Id == id);
  public bool HasCombo(string id) => Combos.Any(a => a.Id == id);
  public bool HasLoader(string id) => Loaders.Any(a => a.Id == id);






  public BaseSource GetSource(string id) => Sources?.FirstOrDefault(f => f.Id == id);
  public BaseAction GetAction(string id) => Actions?.FirstOrDefault(f => f.Id == id);
  public Loader GetLoader(string id) => Loaders?.FirstOrDefault(f => f.Id == id);
  public BaseFormField GetInput(string id) => Inputs?.FirstOrDefault(f => f.Id == id);
  public BaseCombo GetCombo(string id) => Combos?.FirstOrDefault(f => f.Id == id);

}
