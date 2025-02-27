using DiffCode.JSGenerator;
using ZMLProject.Models.Valuables;
using ZMLProject.Locators;






namespace ZMLProject.Services
{
  /// <summary>
  /// Интерфейс сервиса-поставщика JavaScript-выражений.
  /// </summary>
  public interface IJSExpressionsService
  {




    void SetNewAppInfo(string globalvar, string appname);






    /// <summary>
    /// Словарь JavaScript-выражений для поиска свойств в элементах интерфейса.
    /// </summary>
    Dictionary<string, Dictionary<string, Func<GetProp, MemberAccess>>> Prop_Getter { get; }

    /// <summary>
    /// Словарь JavaScript-выражений для установки свойств в элементах интерфейса.
    /// </summary>
    Dictionary<string, Dictionary<string, Func<SetProp, MemberAccess>>> Prop_Setter { get; }



    /// <summary>
    /// Словарь JavaScript-выражений для добавления значения в целевое свойство элементов интерфейса.
    /// </summary>
    Dictionary<string, Dictionary<string, Func<ValueAdder, MemberAccess>>> Value_Adder { get; }

    /// <summary>
    /// Словарь JavaScript-выражений для удаления значения из целевого свойства элементов интерфейса.
    /// </summary>
    Dictionary<string, Dictionary<string, Func<ValueRemover, MemberAccess>>> Value_Remover { get; }















    #region LOCATORS //////////////////////////////////////////////////////////



    MemberAccess LocateByCategory(string type, string id);

    MemberAccess LocateAction(string id);

    MemberAccess LocateSource(string id);

    MemberAccess LocateButton(string id);

    MemberAccess LocateChart(string id);

    MemberAccess LocateCombo(string id);

    MemberAccess LocateComponent(string id);

    MemberAccess LocateDialog(string id);

    MemberAccess LocateEditor(string id);

    MemberAccess LocateForm(string id);

    MemberAccess LocateGrid(string id);

    MemberAccess LocateInput(string id);

    MemberAccess LocateList(string id);

    MemberAccess LocatePanel(string id);

    MemberAccess LocateTabpanel(string id);

    MemberAccess LocateTemplate(string id);

    MemberAccess LocateTree(string id);



    #endregion










    #region OTHER METHODS /////////////////////////////////////////////////////



    MemberAccess GLOBAL_APP();

    MemberAccess APP_VIEW();

    MemberAccess APP_VIEW_DoLayout();





    MemberAccess Ext_application(ObjectBody obj);

    ExtJSClassExpression Ext_define(string name, ObjectBody obj);

    ExtJSClassExpression Ext_define(string name, ObjectBody obj, params string[] comments);

    ExtJSClassExpression Ext_create(string name);

    ExtJSClassExpression Ext_create(string name, ObjectBody obj, params string[] comments);

    MemberAccess Ext_widget(ObjectBody obj);

    MemberAccess Ext_widget(string name, ObjectBody obj);








    MemberAccess Ext_Get(string id);

    MemberAccess Ext_GetStore(string id);

    MemberAccess Ext_GetSoreAndLoad(string id);

    MemberAccess Ext_CreateMenuFromConfig(ObjectBody config);

    MemberAccess Ext_StoreManager_SaveNewRecord(string id, IEnumerable<Param> linkers);

    




    MemberAccess Console_Dir(AbstractExpression expr);



    /// <summary>
    /// Выражение для создания нового экземпляра параметра.
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    ExtJSClassExpression Param_New(Param param);






    #endregion






    GridLocator Grid(string id);
    ComboLocator Combo(string id);
    InputLocator Input(string id);
    SourceLocator Source(string id);
    EditorLocator Editor(string id);
    ListLocator List(string id);
    PanelLocator Panel(string id);
    TreeLocator Tree(string id);
    DialogLocator Dialog(string id);
    ActionLocator Action(string id);
    ButtonLocator Button(string id);
    ChartLocator Chart(string id);
    FormLocator Form(string id);
    TabpanelLocator Tabpanel(string id);
    LoaderLocator Loader(string id);

  }
}
