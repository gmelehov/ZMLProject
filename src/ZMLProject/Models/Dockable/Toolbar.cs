using DiffCode.CommonExtensions;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Models.Forms;
using System.Diagnostics;


namespace ZMLProject.Models.Dockable;

/// <summary>
/// Тулбар.
/// </summary>
public class Toolbar : ZmlContainer, IToolbar
{
  public Toolbar(Docked host, XElement source) : base(host, source)
  {

    foreach (var s in source.Elements(F("Spacer")))
    {
      ProcessSpacer(s);
    };

    _items.AddRange(source.Elements().Select(el =>
    {
      var name = el.Name.LocalName;
      var type = el.AttrVal("type");
      ZmlComponent ret;
      switch (name)
      {
        case "TextBlock": ret = new TextBlock(this, el); break;
        case "Spacer": ret = new Spacer(this, el); break;
        case "Separator": ret = new Separator(this, el); break;

        case "Button": ret = new Button(this, el); break;
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



        default: ret = null; break;
      };
      return ret;
    }).Where(w => w != null));
  }





  public void ProcessSpacer(XElement el)
  {
    var width = el?.AttrVal("width");
    if (width != null)
    {
      var separatorMode = el.AttrVal("separators");
      switch (separatorMode)
      {
        case "left": el.AddBeforeSelf(new XElement(F("Separator"))); break;

        case "right": el.AddAfterSelf(new XElement(F("Separator"))); break;

        case "left-right":
          el.AddBeforeSelf(new XElement(F("Separator")));
          el.AddAfterSelf(new XElement(F("Separator")));
          break;

        case "inside":
          int intWidth;
          bool res = int.TryParse(width, out intWidth);
          if (res == true)
          {
            int mid = (int)Math.Round((double)intWidth / 2);
            el.SetAttributeValue("width", mid);
            el.AddAfterSelf(new XElement(F("Spacer"), new XAttribute("width", mid)));
            el.AddAfterSelf(new XElement(F("Separator")));
          };
          break;


        default: break;
      };
    };
  }







  public override ExtJSClassExpression AsDefine => null;
  public override ObjectBody AsConfig => OBJ(
    KV_XType,
    KV_Dock,
    KV("items", Arr(false, true, Items?.Select(f => f.AsConfig)))
    );







  public string XType => App.bsrv.Toolbar(this).XType;

  public string Dock => this["dock"];
  
  public IEnumerable<IButton> Buttons => Items?.OfType<IButton>();










  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_XType => KV("xtype", XType);

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  public KeyValue KV_Dock => KV("dock", Dock);
}
