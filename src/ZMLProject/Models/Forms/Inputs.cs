using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;
using ZMLProject.Models.Dockable;
using DiffCode.CommonExtensions.Attributes;





namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Группа полей ввода данных.
  /// </summary>
  public class Inputs : BaseFormItem, IHasXType
  {
    public Inputs(IZmlContainer form, XElement source) : base(form, source)
    {
      _items.AddRange(source.Elements().Select(el =>
      {
        var name = el.Name.LocalName;
        IZmlComponent ret;
        switch (name)
        {
          case "Combo": ret = new Combobox(this, el); break;
          case "Checkbox": ret = new Checkbox(this, el); break;
          case "NumberField": ret = new NumberField(this, el); break;
          case "NumberSet": ret = new NumberSet(this, el); break;
          case "DateField": ret = new DateField(this, el); break;
          case "DateSet": ret = new DateSet(this, el); break;
          case "TimeField": ret = new TimeField(this, el); break;
          case "MonthCombo": ret = new MonthCombo(this, el); break;
          case "TextField": ret = new TextField(this, el); break;
          case "TextArea": ret = new TextArea(this, el); break;
          case "KeyValueInput": ret = new KeyValueInput(this, el); break;
          //case "PairedCombo": ret = new 
          case "Collection": ret = new Collection(this, el); break;
          case "Button": ret = new Button(this, el); break;


          default: ret = null; break;
        };
        return ret;
      })
      .Where(w => w != null));
    }







    public override ObjectBody AsConfig => OBJ(
      KV_XType,
      KV_ItemId,
      KV_Title,
      KV_ColumnWidth,
      KV_Collapsible,
      KV_Cls,
      KV_Layout,
      KV_Defaults,
      KV_Items
      );







    public string Id => this["id"];
    public string XType => App.bsrv.Inputs(this).XType;
    public bool Collapsible => App.bsrv.Inputs(this).Collapsible;
    public string Title => this["title"];
    public override string Cls => App.bsrv.Inputs(this).Cls;
    public string Layout => App.bsrv.Inputs(this).Layout;








    public KeyValue KV_ItemId => PROP("itemId", Id);
    public KeyValue KV_XType => KV("xtype", XType);
    public KeyValue KV_Collapsible => PROP("collapsible", Collapsible);
    public KeyValue KV_Title => PROP("title", Title);
    public KeyValue KV_Layout => PROP("layout", Layout);
    
    
    [ToDo("Вынести определения в сервисы")]
    public KeyValue KV_Defaults => KV("defaults", OBJ(false, KV("anchor", "100%"), KV("margin", "0 0 5 5")));

    public KeyValue KV_Items => KV("items", Arr(false, true, Items.OfType<BaseFormField>().Select(s => s.AsConfig)));
  }
}
