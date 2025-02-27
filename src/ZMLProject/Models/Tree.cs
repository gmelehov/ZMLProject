using System.Xml.Linq;
using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using ZMLProject.Models.Plugins;





namespace ZMLProject.Models
{
  /// <summary>
  /// Таблица с древовидным отображением данных.
  /// </summary>
  public class Tree : BasePanel, IHasPlugins
  {
    public Tree(ZmlContainer host, XElement source) : base(host, source)
    {

      if (Source != null)
      {
        Source.ConnectToTree(this);
      };

      ProcessDockedItems(source);

      _plugins.Add(new GridEditingPlugin(this));
    }






    public override ExtJSClassExpression AsDefine => null;
    protected override ObjectBody SpecificKeyValues => OBJ(
      KV_Store,
      KV_SelModel,
      KV_RowCls,
      KV_FieldsForRowCls,
      KV_ExtraToolbars,
      KV_HideHeaders,
      KV_ForceFit,
      KV_Expanded,
      KV_AppNAME,
      KV_Features,
      KV_Plugins,
      KV_Columns
      );








    public BaseSource Source => Closest<ZmlApplication>()?.Sources.FirstOrDefault(f => f.Id == Src);









    public override string XType => App.bsrv.Tree(this).XType;
    
    public string Src => this["src"];
    
    public string Create => this["create"];
    
    public string Update => this["update"];
    
    public string Delete => this["delete"];
    
    public string StoreId => $"{Src}";
    
    public bool? HideHeaders => parseBoolAttr("hideHeaders");
    
    public bool? ForceFit => parseBoolAttr("forceFit");
    
    public bool Expanded => App.bsrv.Tree(this).Expanded;
    
    public string Sel => App.bsrv.Tree(this).Sel;
    
    public string SelMode => App.bsrv.Tree(this).SelMode;
    
    public string RowCls => this["rowCls"];
    
    public IEnumerable<string> FieldsForRowCls => Source?.Fields?.Where(w => w.IsForRecordCls).Select(s => s.Id);
    
    public override string Layout => null;

    /// <summary>
    /// Тип плагина для инлайн-редактирования узлов дерева.
    /// </summary>
    public string Editor => App.bsrv.Tree(this).Editor;

    /// <summary>
    /// Идентификатор плагина для инлайн-редактирования узлов дерева.
    /// </summary>
    public string EditorId => App.bsrv.Tree(this).EditorId;












    public override KeyValue KV_XType => KV("xtype", XType);
    
    public KeyValue KV_Store => KV("store", StoreId);
    
    public KeyValue KV_RowCls => PROP("rowCls", RowCls);
    
    public KeyValue KV_FieldsForRowCls => KV("fieldsForRowCls", Arr(false, false, FieldsForRowCls?.Select(s => Str(s))));
    
    public KeyValue KV_Columns =>
      KV("columns",
        OBJ(
          KV("defaults", OBJ(false, KV("filterable", TRUE))),
          KV("items", Arr(false, true, Source?.Columns))
          ));
    
    public override KeyValue KV_Docked => KV("dockedItems",
      Docked != null ?
      Arr(false, true,
        Docked?.Forms?.Select(t => t?.AsConfig)
        )
      :
      null
      );
    
    public KeyValue KV_ExtraToolbars => KV("ExtraToolbars", Docked != null ? Arr(false, true, Docked?.Toolbars?.Select(t => t?.AsConfig)) : Arr());
    
    public KeyValue KV_SelModel => SelMode != null ?
      KV("selModel",
        OBJ(false, false,
          KV("selType", "treemodel"),
          KV("allowDeselect", TRUE),
          KV("mode", SelMode)
          )
        )
      : null;
    
    public KeyValue KV_Expanded => KV("expanded", Expanded);
    
    public override KeyValue KV_Layout => null;
    
    public KeyValue KV_HideHeaders => PROP("hideHeaders", HideHeaders);
    
    public KeyValue KV_ForceFit => PROP("forceFit", ForceFit);
    
    private KeyValue KV_AppNAME => KV("AppNAME", App.Name);
    
    public KeyValue KV_Features => FeaturesArray.Count() > 0 ? KV("features", Arr(false, false, FeaturesArray)) : null;



    private ObjectBody KV_Filtering_Feature => OBJ(false, false, KV("ftype", "TreeFilters"), KV("encode", true));
    
    private IEnumerable<ObjectBody> FeaturesArray => new ObjectBody[] { KV_Filtering_Feature };





    private bool SourceHasParams => Source != null && Source.Params.Count() > 0;
  }
}
