using DiffCode.JSGenerator;

using ZMLProject.Enums;
using ZMLProject.Locators;
using ZMLProject.Models.Valuables;

using static DiffCode.JSGenerator.Generator;


namespace ZMLProject.Services;

/// <summary>
/// Сервис-поставщик JavaScript-выражений.
/// </summary>
public class JSExpressionsService : IJSExpressionsService
{
  private string globalvar;
  private string appname;





  public JSExpressionsService(string globalVar = "GLOBAL", string appName = "APP")
  {
    globalvar = globalVar;
    appname = appName;
  }







  public void SetNewAppInfo(string globalVar, string appName)
  {
    globalvar = globalVar;
    appname = appName;
  }








  public Identifier GLOBAL => ID(globalvar);
  public Identifier APP => ID(appname);
  public Identifier VIEW => ID("VIEW");
  public MemberAccess GLOBAL_APP_VIEW => new MemberAccess(GLOBAL, APP, VIEW);


  
  public Identifier EXT => ID("Ext");
  public Identifier STOREMANAGER => ID("StoreManager");




  public MemberAccess Locator(string type) => new MemberAccess(GLOBAL, APP, ID(type));
  public MemberAccess ActionCaller(string actionId) => Locator("Action").AddExpr(CALL(actionId, Args()));
  public MemberAccess ActionCaller(string actionId, ArgumentsBlock arg) => Locator("Action").AddExpr(CALL(actionId, arg));














  public MemberAccess LocateByCategory(string type, string id) => Locator(type).AddExpr(ID(id));
  public MemberAccess LocateAction(string id) => Locator("Action").AddExpr(CALL("get", Args(Str(id))));
  public MemberAccess LocateSource(string id) => Locator("Source").AddExpr(ID(id));
  public MemberAccess LocateButton(string id) => Locator("Button").AddExpr(ID(id));
  public MemberAccess LocateChart(string id) => Locator("Chart").AddExpr(ID(id));
  public MemberAccess LocateCombo(string id) => Locator("Combo").AddExpr(ID(id));
  public MemberAccess LocateComponent(string id) => Locator("Component").AddExpr(ID(id));
  public MemberAccess LocateDialog(string id) => Locator("Dialog").AddExpr(ID(id));
  public MemberAccess LocateEditor(string id) => Locator("Editor").AddExpr(ID(id));
  public MemberAccess LocateForm(string id) => Locator("Form").AddExpr(ID(id));
  public MemberAccess LocateGrid(string id) => Locator("Grid").AddExpr(ID(id));
  public MemberAccess LocateInput(string id) => Locator("Input").AddExpr(ID(id));
  public MemberAccess LocateList(string id) => Locator("List").AddExpr(ID(id));
  public MemberAccess LocatePanel(string id) => Locator("Panel").AddExpr(ID(id));
  public MemberAccess LocateTabpanel(string id) => Locator("Tabpanel").AddExpr(ID(id));
  public MemberAccess LocateTemplate(string id) => Locator("Template").AddExpr(ID(id));
  public MemberAccess LocateTree(string id) => Locator("Tree").AddExpr(ID(id));





  //public NewExpr Param_New(Param param) => new NewExpr(new MemberAccess(param.Classname, CALL(Args(param.ConfigObject))));
  public ExtJSClassExpression Param_New(Param param) => new ExtJSClassExpression(param.Classname, param.ConfigObject, true);




  public MemberAccess GLOBAL_APP() => new MemberAccess(GLOBAL, APP);
  public MemberAccess APP_VIEW() => GLOBAL_APP_VIEW;
  public MemberAccess APP_VIEW_DoLayout() => GLOBAL_APP_VIEW.AddExpr(CALL("doLayout", Args()));




  public MemberAccess Ext_Get(string id) => new MemberAccess(EXT, CALL("get", Args(Str(id))));
  public MemberAccess Ext_GetStore(string id) => new MemberAccess(EXT, CALL("getStore", Args(Str(id))));
  public MemberAccess Ext_GetSoreAndLoad(string id) => new MemberAccess(Ext_GetStore(id), CALL("load", Args()));
  public MemberAccess Ext_CreateMenuFromConfig(ObjectBody config) => new MemberAccess(EXT, ID("menu"), CALL("Menu", Args(config)));
  public MemberAccess Console_Dir(AbstractExpression expr) => new MemberAccess(ID("console"), CALL("dir", Args(expr)));



  public MemberAccess Ext_application(ObjectBody obj) => new MemberAccess(EXT, CALL("application", Args(obj)));
  public ExtJSClassExpression Ext_define(string name, ObjectBody obj) => new ExtJSClassExpression(name, obj, false);
  public ExtJSClassExpression Ext_define(string name, ObjectBody obj, params string[] comments) => (comments != null && comments.Length > 0) ? new ExtJSClassExpression(name, obj, false).AddCommentBlock(comments) : new ExtJSClassExpression(name, obj, false);
  public ExtJSClassExpression Ext_create(string name) => new ExtJSClassExpression(name, null, true);
  public ExtJSClassExpression Ext_create(string name, ObjectBody obj, params string[] comments) => (comments != null && comments.Length > 0) ? new ExtJSClassExpression(name, obj, true).AddCommentBlock(comments) : new ExtJSClassExpression(name, obj, true);
  public MemberAccess Ext_widget(ObjectBody obj) => new MemberAccess(EXT, CALL("widget", Args(obj)));
  public MemberAccess Ext_widget(string name, ObjectBody obj) => new MemberAccess(EXT, CALL("widget", Args(Str(name), obj)));
  public MemberAccess Ext_StoreManager_SaveNewRecord(string id, IEnumerable<Param> linkers) => new MemberAccess(EXT, STOREMANAGER, CALL("SAVENEWRECORD", Args(Str(id), Arr(linkers.Select(s => s.AsExprs)))));









  public Dictionary<string, Dictionary<string, Func<GetProp, MemberAccess>>> Prop_Getter => new Dictionary<string, Dictionary<string, Func<GetProp, MemberAccess>>>
  {
    [ElTypes.Grid] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.SelCount] = gp => Grid(gp.ScopeId).GetSelectedCount(),
      [ElProps.SelectedKey] = gp => Grid(gp.ScopeId).GetSelectedKey(),
      [ElProps.Key] = gp => Grid(gp.ScopeId).GetKey(),
      [ElProps.Src] = gp => Grid(gp.ScopeId).GetSrc(),
      [ElProps.Value] = gp => Grid(gp.ScopeId).GetRecord(gp.Name, gp.Record)
    },
    [ElTypes.DateField] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
      [ElProps.Day] = gp => Input(gp.ScopeId).GetDay(),
      [ElProps.Month] = gp => Input(gp.ScopeId).GetMonth(),
      [ElProps.Year] = gp => Input(gp.ScopeId).GetYear(),
    },
    [ElTypes.TimeField] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
      [ElProps.Hours] = gp => Input(gp.ScopeId).GetHours(),
      [ElProps.Minutes] = gp => Input(gp.ScopeId).GetMinutes(),
      [ElProps.Seconds] = gp => Input(gp.ScopeId).GetSeconds(),
    },
    [ElTypes.NumberField] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
      [ElProps.Step] = gp => Input(gp.ScopeId).GetStep(),
      [ElProps.MinValue] = gp => Input(gp.ScopeId).GetMinValue(),
      [ElProps.MaxValue] = gp => Input(gp.ScopeId).GetMaxValue(),
    },
    [ElTypes.CheckBox] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
    },
    [ElTypes.TextField] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
    },
    [ElTypes.TextArea] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
    },
    [ElTypes.Editor] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Value] = gp => Editor(gp.ScopeId).GetValue(),
      [ElProps.Mode] = gp => Editor(gp.ScopeId).GetMode(),
      [ElProps.HtmlContent] = gp => Editor(gp.ScopeId).GetHtmlContent(),
    },
    [ElTypes.List] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.SelCount] = gp => List(gp.ScopeId).CountSelected(),
      [ElProps.TotalCount] = gp => List(gp.ScopeId).CountTotal(),
      [ElProps.Value] = gp => List(gp.ScopeId).GetRecord(gp.Name, gp.Record),
    },
    [ElTypes.ComboBox] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.DescrValue] = gp => Combo(gp.ScopeId).GetDescriptionValue(),
      [ElProps.DisplayValue] = gp => Combo(gp.ScopeId).GetDisplayValue(),
      [ElProps.TotalCount] = gp => Combo(gp.ScopeId).CountTotal(),
      [ElProps.Value] = gp => Combo(gp.ScopeId).GetValue(),
    },
    [ElTypes.Source] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.TotalCount] = gp => Source(gp.ScopeId).GetTotalCount(),
      [ElProps.PageSize] = gp => Source(gp.ScopeId).GetPageSize(),
      [ElProps.TotalPages] = gp => Source(gp.ScopeId).GetTotalPages(),
      [ElProps.CurrentPage] = gp => Source(gp.ScopeId).GetCurrentPage(),
      [ElProps.Value] = gp => Source(gp.ScopeId).GetRecord(gp.Name, gp.Record)
    },
    [ElTypes.KeyValueInput] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.KeyPart] = gp => Input(gp.ScopeId).GetKeyPart(),
      [ElProps.ValuePart] = gp => Input(gp.ScopeId).GetValuePart(),
      [ElProps.Value] = gp => Input(gp.ScopeId).GetValue(),
    },
    [ElTypes.Panel] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Collapsed] = gp => Panel(gp.ScopeId).GetCollapsed(),
      [ElProps.Loader] = gp => Panel(gp.ScopeId).GetLoader(),
    },
    [ElTypes.Loader] = new Dictionary<string, Func<GetProp, MemberAccess>>
    {
      [ElProps.Url] = gp => Loader(gp.ScopeId).GetUrl(),
      [ElProps.Mode] = gp => Loader(gp.ScopeId).GetMode(),
    },


  };


  public Dictionary<string, Dictionary<string, Func<SetProp, MemberAccess>>> Prop_Setter => new Dictionary<string, Dictionary<string, Func<SetProp, MemberAccess>>>
  {
    [ElTypes.Editor] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Mode] = sp => Editor(sp.TargetId).SetMode(sp.MEMB_Source),
      [ElProps.Value] = sp => Editor(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.TextField] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Value] = sp => Input(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.TextArea] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Value] = sp => Input(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.DateField] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Day] = sp => Input(sp.TargetId).SetDay(sp.MEMB_Source),
      [ElProps.Month] = sp => Input(sp.TargetId).SetMonth(sp.MEMB_Source),
      [ElProps.Year] = sp => Input(sp.TargetId).SetYear(sp.MEMB_Source),
      [ElProps.Value] = sp => Input(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.DateSet] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.FromValue] = sp => Input(sp.TargetId).SetFromValue(sp.MEMB_Source),
      [ElProps.ToValue] = sp => Input(sp.TargetId).SetToValue(sp.MEMB_Source),
    },
    [ElTypes.NumberSet] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.FromValue] = sp => Input(sp.TargetId).SetFromValue(sp.MEMB_Source),
      [ElProps.ToValue] = sp => Input(sp.TargetId).SetToValue(sp.MEMB_Source),
    },
    [ElTypes.TimeField] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Hours] = sp => Input(sp.TargetId).SetHours(sp.MEMB_Source),
      [ElProps.Minutes] = sp => Input(sp.TargetId).SetMinutes(sp.MEMB_Source),
      [ElProps.Seconds] = sp => Input(sp.TargetId).SetSeconds(sp.MEMB_Source),
      [ElProps.Value] = sp => Input(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.NumberField] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Step] = sp => Input(sp.TargetId).SetStep(sp.MEMB_Source),
      [ElProps.MinValue] = sp => Input(sp.TargetId).SetMinValue(sp.MEMB_Source),
      [ElProps.MaxValue] = sp => Input(sp.TargetId).SetMaxValue(sp.MEMB_Source),
      [ElProps.Value] = sp => Input(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.ComboBox] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Value] = sp => Combo(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.CheckBox] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Value] = sp => Input(sp.TargetId).SetValue(sp.MEMB_Source),
    },
    [ElTypes.KeyValueInput] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.KeyPart] = sp => Input(sp.TargetId).SetKeyPart(sp.MEMB_Source),
      [ElProps.ValuePart] = sp => Input(sp.TargetId).SetValuePart(sp.MEMB_Source),
      [ElProps.Delimiter] = sp => Input(sp.TargetId).SetDelimiter(sp.MEMB_Source),
    },
    [ElTypes.Panel] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Collapsed] = sp => Panel(sp.TargetId).SetCollapsed(sp.MEMB_Source),
      [ElProps.Loader] = sp => Panel(sp.TargetId).SetLoader(sp.MEMB_Source),
    },
    [ElTypes.Loader] = new Dictionary<string, Func<SetProp, MemberAccess>>
    {
      [ElProps.Url] = sp => Loader(sp.TargetId).SetUrl(sp.MEMB_Source),
    },

  };






  public Dictionary<string, Dictionary<string, Func<ValueAdder, MemberAccess>>> Value_Adder => new Dictionary<string, Dictionary<string, Func<ValueAdder, MemberAccess>>>
  {
    [ElTypes.Panel] = new Dictionary<string, Func<ValueAdder, MemberAccess>>
    {
      [ElProps.Cls] = va => Panel(va.TargetId).AddCls(va.MEMB_Source),
      [ElProps.BodyCls] = va => Panel(va.TargetId).AddBodyCls(va.MEMB_Source),
      [ElProps.HtmlContent] = va => Panel(va.TargetId).AddHtmlContent(va.MEMB_Source),
    },
    [ElTypes.Form] = new Dictionary<string, Func<ValueAdder, MemberAccess>>
    {
      [ElProps.Cls] = va => Form(va.TargetId).AddCls(va.MEMB_Source),
      [ElProps.BodyCls] = va => Form(va.TargetId).AddBodyCls(va.MEMB_Source),
    },
    [ElTypes.Dialog] = new Dictionary<string, Func<ValueAdder, MemberAccess>>
    {
      [ElProps.Cls] = va => Dialog(va.TargetId).AddCls(va.MEMB_Source),
      [ElProps.BodyCls] = va => Dialog(va.TargetId).AddBodyCls(va.MEMB_Source),
    }
  };


  public Dictionary<string, Dictionary<string, Func<ValueRemover, MemberAccess>>> Value_Remover => new Dictionary<string, Dictionary<string, Func<ValueRemover, MemberAccess>>>
  {
    [ElTypes.Panel] = new Dictionary<string, Func<ValueRemover, MemberAccess>>
    {
      [ElProps.Cls] = vr => Panel(vr.TargetId).RemoveCls(vr.MEMB_Source),
      [ElProps.BodyCls] = vr => Panel(vr.TargetId).RemoveBodyCls(vr.MEMB_Source),
      [ElProps.HtmlContent] = vr => Panel(vr.TargetId).RemoveHtmlContent(),
    },
    [ElTypes.Form] = new Dictionary<string, Func<ValueRemover, MemberAccess>>
    {
      [ElProps.Cls] = vr => Form(vr.TargetId).RemoveCls(vr.MEMB_Source),
      [ElProps.BodyCls] = vr => Form(vr.TargetId).RemoveBodyCls(vr.MEMB_Source),
    },
    [ElTypes.Dialog] = new Dictionary<string, Func<ValueRemover, MemberAccess>>
    {
      [ElProps.Cls] = vr => Dialog(vr.TargetId).RemoveCls(vr.MEMB_Source),
      [ElProps.BodyCls] = vr => Dialog(vr.TargetId).RemoveBodyCls(vr.MEMB_Source),
    }
  };
























  public GridLocator Grid(string id) => new GridLocator(globalvar, appname, id);
  public ComboLocator Combo(string id) => new ComboLocator(globalvar, appname, id);
  public InputLocator Input(string id) => new InputLocator(globalvar, appname, id);
  public SourceLocator Source(string id) => new SourceLocator(globalvar, appname, id);
  public EditorLocator Editor(string id) => new EditorLocator(globalvar, appname, id);
  public ListLocator List(string id) => new ListLocator(globalvar, appname, id);
  public PanelLocator Panel(string id) => new PanelLocator(globalvar, appname, id);
  public TreeLocator Tree(string id) => new TreeLocator(globalvar, appname, id);
  public DialogLocator Dialog(string id) => new DialogLocator(globalvar, appname, id);
  public ActionLocator Action(string id) => new ActionLocator(globalvar, appname, id);
  public ButtonLocator Button(string id) => new ButtonLocator(globalvar, appname, id);
  public ChartLocator Chart(string id) => new ChartLocator(globalvar, appname, id);
  public FormLocator Form(string id) => new FormLocator(globalvar, appname, id);
  public TabpanelLocator Tabpanel(string id) => new TabpanelLocator(globalvar, appname, id);
  public LoaderLocator Loader(string id) => new LoaderLocator(globalvar, appname, id);


}
