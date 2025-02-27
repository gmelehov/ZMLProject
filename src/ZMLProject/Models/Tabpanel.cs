using System.Xml.Linq;

using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models
{
  /// <summary>
  /// Панель со вкладками.
  /// </summary>
  public class Tabpanel : BasePanel
  {
    public Tabpanel(ZmlContainer host, XElement source) : base(host, source)
    {

      ProcessCommonAttributes();

      ProcessCommonElements(source);

      ProcessDockedItems(source);

    }








    public override ExtJSClassExpression AsDefine => null;
    protected override ObjectBody SpecificKeyValues => OBJ(
      KV("AppNAME", App.Name),
      PROP("tabPosition", TabPosition),
      KV_Plugins
      );











    public override string XType => App.bsrv.Tabpanel(this).XType;


    public override string Layout => App.bsrv.Tabpanel(this).Layout;
      
    
    public string TabPosition => this["tabPosition"];











    public override KeyValue KV_XType => KV("xtype", XType);
    
    
    public override KeyValue KV_Layout =>
      Layout == "accordion" ?
      KV("layout", OBJ(false, false,
        KV("type", "accordion"),
        KV("multi", this["layout"] == "multiaccordion"),
        KV("animate", TRUE)
        ))
      :
      PROP("layout", this["layout"]);

  }
}
