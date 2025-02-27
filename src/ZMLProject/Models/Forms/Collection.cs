using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;







namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// 
  /// </summary>
  public class Collection : BaseFormField
  {
    public Collection(IZmlContainer host, XElement source) : base(host, source)
    {
      
    }











    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_KVDelimiter,
      KV_ItemsDelimiter,
      KV_ItemsType,
      KV_UniqueItems,
      KV_ShowIndices,
      KV_SubmitAs
      );











    public override string XType => App.bsrv.Collection(this).XType;
    
    public string KVDelimiter => this["kvDelimiter"];
    
    public string ItemsDelimiter => this["itemsDelimiter"];
    
    public string ItemsType => this["itemsType"];
    
    public bool? ShowIndices => parseBoolAttr("showIndices");
    
    public bool? UniqueItems => parseBoolAttr("uniqueItems");
    
    public string SubmitAs => this["submit-as"];












    public KeyValue KV_UniqueItems => PROP("uniqueItems", UniqueItems);
    
    public KeyValue KV_ShowIndices => PROP("showIndices", ShowIndices);
    
    public KeyValue KV_KVDelimiter => PROP("kvDelimiter", KVDelimiter);
    
    public KeyValue KV_ItemsDelimiter => PROP("itemsDelimiter", ItemsDelimiter);
    
    public KeyValue KV_ItemsType => PROP("itemsType", ItemsType);
    
    public KeyValue KV_SubmitAs => PROP("submitAs", SubmitAs);

  }
}
