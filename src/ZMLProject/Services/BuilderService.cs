using ZMLProject.Builders;
using ZMLProject.Models.Dockable;
using ZMLProject.Models.Forms;
using ZMLProject.Models.Grids;
using ZMLProject.Models.Plugins;





namespace ZMLProject.Services
{
  /// <summary>
  /// Сервис-поставщик данных для построения элементов разметки.
  /// </summary>
  public class BuilderService : IBuilderService
  {


    public ButtonBuilder Button(Button button) => new ButtonBuilder(button);
    public BaseComboBuilder BaseCombo(BaseCombo baseCombo) => new BaseComboBuilder(baseCombo);
    public RestSourceBuilder RestSource(RestSource restSource) => new RestSourceBuilder(restSource);
    public LocalSourceBuilder LocalSource(LocalSource localSource) => new LocalSourceBuilder(localSource);
    public XmlSourceBuilder XmlSource(XmlSource xmlSource) => new XmlSourceBuilder(xmlSource);
    public BaseSourceBuilder BaseSource(BaseSource baseSource) => new BaseSourceBuilder(baseSource);
    public BaseActionBuilder BaseAction(BaseAction baseAction) => new BaseActionBuilder(baseAction);
    public SourceModelBuilder SourceModel(SourceModel sourceModel) => new SourceModelBuilder(sourceModel);
    public ParamBuilder Param(Param param) => new ParamBuilder(param);
    public MemoryProxyBuilder MemoryProxy(MemoryProxy memoryProxy) => new MemoryProxyBuilder(memoryProxy);
    public RestProxyBuilder RestProxy(RestProxy restProxy) => new RestProxyBuilder(restProxy);
    public XmlProxyBuilder XmlProxy(XmlProxy xmlProxy) => new XmlProxyBuilder(xmlProxy);
    public RestActionBuilder RestAction(RestAction restAction) => new RestActionBuilder(restAction);
    public ZmlApplicationBuilder ZmlApplication(ZmlApplication zmlApplication) => new ZmlApplicationBuilder(zmlApplication);
    public SubMenuBuilder SubMenu(SubMenu subMenu) => new SubMenuBuilder(subMenu);
    public ViewPortBuilder ViewPort(ViewPort viewPort) => new ViewPortBuilder(viewPort);
    public ToolbarBuilder Toolbar(Toolbar toolbar) => new ToolbarBuilder(toolbar);
    public SeparatorBuilder Separator(Separator separator) => new SeparatorBuilder(separator);
    public SpacerBuilder Spacer(Spacer spacer) => new SpacerBuilder(spacer);
    public TextBlockBuilder TextBlock(TextBlock textBlock) => new TextBlockBuilder(textBlock);
    public HostContextPluginBuilder HostContextPlugin(HostContextPlugin hostContextPlugin) => new HostContextPluginBuilder(hostContextPlugin);
    public PanelBuilder Panel(Panel panel) => new PanelBuilder(panel);
    public DialogBuilder Dialog(Dialog dialog) => new DialogBuilder(dialog);
    public InputsBuilder Inputs(Inputs inputs) => new InputsBuilder(inputs);
    public FormBuilder Form(Form form) => new FormBuilder(form);
    public SplitterBuilder Splitter(Splitter splitter) => new SplitterBuilder(splitter);
    public GridBuilder Grid(Grid grid) => new GridBuilder(grid);
    public GridPagingBuilder GridPaging(GridPaging gridPaging) => new GridPagingBuilder(gridPaging);
    public ListBuilder List(List list) => new ListBuilder(list);
    public TabpanelBuilder Tabpanel(Tabpanel tabpanel) => new TabpanelBuilder(tabpanel);
    public TreeBuilder Tree(Tree tree) => new TreeBuilder(tree);
    public KeyValueInputBuilder KeyValueInput(KeyValueInput keyValueInput) => new KeyValueInputBuilder(keyValueInput);
    public CheckboxBuilder Checkbox(Checkbox checkbox) => new CheckboxBuilder(checkbox);
    public CollectionBuilder Collection(Collection collection) => new CollectionBuilder(collection);
    public DateFieldBuilder DateField(DateField dateField) => new DateFieldBuilder(dateField);
    public DateSetBuilder DateSet(DateSet dateSet) => new DateSetBuilder(dateSet);
    public NumberFieldBuilder NumberField(NumberField numberField) => new NumberFieldBuilder(numberField);
    public NumberSetBuilder NumberSet(NumberSet numberSet) => new NumberSetBuilder(numberSet);
    public TextAreaBuilder TextArea(TextArea textArea) => new TextAreaBuilder(textArea);
    public TextFieldBuilder TextField(TextField textField) => new TextFieldBuilder(textField);
    public TimeFieldBuilder TimeField(TimeField timeField) => new TimeFieldBuilder(timeField);
    public ComboboxBuilder Combobox(Combobox combobox) => new ComboboxBuilder(combobox);
    public MonthComboBuilder MonthCombo(MonthCombo monthCombo) => new MonthComboBuilder(monthCombo);
    public EditorBuilder Editor(Editor editor) => new EditorBuilder(editor);
    public BaseFormFieldBuilder BaseFormField(BaseFormField baseFormField) => new BaseFormFieldBuilder(baseFormField);
    public MenuItemBuilder MenuItem(MenuItem menuItem) => new MenuItemBuilder(menuItem);
    public LocalActionBuilder LocalAction(LocalAction localAction) => new LocalActionBuilder(localAction);
    public BasePanelBuilder BasePanel(BasePanel basePanel) => new BasePanelBuilder(basePanel);
    public LoaderBuilder Loader(Loader loader) => new LoaderBuilder(loader);

  }
}
