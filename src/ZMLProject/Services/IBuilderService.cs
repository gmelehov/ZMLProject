using ZMLProject.Builders;
using ZMLProject.Models.Dockable;
using ZMLProject.Models.Forms;
using ZMLProject.Models.Grids;
using ZMLProject.Models.Plugins;


namespace ZMLProject.Services
{
  /// <summary>
  /// Интерфейс сервиса-поставщика данных для построения элементов разметки.
  /// </summary>
  public interface IBuilderService
  {


    ButtonBuilder Button(Button button);
    BaseComboBuilder BaseCombo(BaseCombo baseCombo);
    RestSourceBuilder RestSource(RestSource restSource);
    LocalSourceBuilder LocalSource(LocalSource localSource);
    XmlSourceBuilder XmlSource(XmlSource xmlSource);
    BaseSourceBuilder BaseSource(BaseSource baseSource);
    BaseActionBuilder BaseAction(BaseAction baseAction);
    SourceModelBuilder SourceModel(SourceModel sourceModel);
    ParamBuilder Param(Param param);
    MemoryProxyBuilder MemoryProxy(MemoryProxy memoryProxy);
    RestProxyBuilder RestProxy(RestProxy restProxy);
    XmlProxyBuilder XmlProxy(XmlProxy xmlProxy);
    RestActionBuilder RestAction(RestAction restAction);
    ZmlApplicationBuilder ZmlApplication(ZmlApplication zmlApplication);
    SubMenuBuilder SubMenu(SubMenu subMenu);
    ViewPortBuilder ViewPort(ViewPort viewPort);
    ToolbarBuilder Toolbar(Toolbar toolbar);
    SeparatorBuilder Separator(Separator separator);
    SpacerBuilder Spacer(Spacer spacer);
    TextBlockBuilder TextBlock(TextBlock textBlock);
    HostContextPluginBuilder HostContextPlugin(HostContextPlugin hostContextPlugin);
    PanelBuilder Panel(Panel panel);
    DialogBuilder Dialog(Dialog dialog);
    InputsBuilder Inputs(Inputs inputs);
    FormBuilder Form(Form form);
    SplitterBuilder Splitter(Splitter splitter);
    GridBuilder Grid(Grid grid);
    GridPagingBuilder GridPaging(GridPaging gridPaging);
    ListBuilder List(List list);
    TabpanelBuilder Tabpanel(Tabpanel tabpanel);
    TreeBuilder Tree(Tree tree);
    KeyValueInputBuilder KeyValueInput(KeyValueInput keyValueInput);
    CheckboxBuilder Checkbox(Checkbox checkbox);
    CollectionBuilder Collection(Collection collection);
    DateFieldBuilder DateField(DateField dateField);
    DateSetBuilder DateSet(DateSet dateSet);
    NumberFieldBuilder NumberField(NumberField numberField);
    NumberSetBuilder NumberSet(NumberSet numberSet);
    TextAreaBuilder TextArea(TextArea textArea);
    TextFieldBuilder TextField(TextField textField);
    TimeFieldBuilder TimeField(TimeField timeField);
    ComboboxBuilder Combobox(Combobox combobox);
    MonthComboBuilder MonthCombo(MonthCombo monthCombo);
    EditorBuilder Editor(Editor editor);
    BaseFormFieldBuilder BaseFormField(BaseFormField baseFormField);
    MenuItemBuilder MenuItem(MenuItem menuItem);
    LocalActionBuilder LocalAction(LocalAction localAction);
    BasePanelBuilder BasePanel(BasePanel basePanel);
    LoaderBuilder Loader(Loader loader);

  }
}
