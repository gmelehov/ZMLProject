using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;
using ZMLProject.Models.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DiffCode.CommonExtensions.Attributes;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Поле модели данных.
  /// </summary>
  public class Field : BaseField, IHasPlugins
  {
    private List<IPlugin> _plugins = new List<IPlugin>();





    public Field(IZmlContainer host, XElement source) : base(host, source)
    {
      ProcessFieldData();
      _items.AddRange(source.Elements().Where(w => w.Name.LocalName == "Icon").Select(s => new Icon(this, s)).Where(w => w != null));
    }






    public void ProcessFieldData()
    {
      /// Обработка сокращенного определения Nullable-типа.
      if (this["type"] != null && this["type"].EndsWith("?"))
      {
        _source.SetAttributeValue("type", this["type"].TrimEnd('?'));
        _source.SetAttributeValue("nullable", true);
      };

      if (Filter == "list") _plugins.Add(new ListFilteredColumnPlugin(this));
      if (Filter == "month") _plugins.Add(new MonthFilteredColumnPlugin(this));
      if (Editor == "combobox") _plugins.Add(new ColumnComboEditorPlugin(this));

    }






    /// <summary>
    /// Представление исходного элемента разметки в качестве 
    /// конфигуратора поля в определении модели для хранилища данных.
    /// </summary>
    public override ObjectBody AsConfig => OBJ(false,
      KV_ItemId,
      KV_Type,
      KV_DateFormat,
      KV_UseNull,
      KV_Mapping,
      KV_Persist,
      KV_Default,
      KV_Convert,
      GetRendererFromFormat("serialize")
      );

    /// <summary>
    /// Представление исходного элемента разметки в качестве
    /// конфигуратора столбца таблицы/дерева узлов.
    /// </summary>
    public override ObjectBody Column => Name != null ? OBJ(IconLinkers != null,
      KV_Text,
      KV_DataIndex,
      GetXTypeFromField() == "gridcolumn" || GetXTypeFromField() == null ? null : KV("xtype", GetXTypeFromField()),
      PROP("width", Width),
      PROP("minWidth", MinWidth),
      PROP("maxWidth", MaxWidth),
      PROP("flex", Flex),
      PROP("align", Align),
      KV_Draggable,
      KV_Filterable,
      KV_Sortable,
      GetRendererFromFormat("renderer") != null ? GetXTypeFromField() == "numbercol" ? PROP("format", "''") : null : PROP("format", Format),
      PROP("tooltip", Tooltip),
      PROP("tdCls", Cls),
      PROP("summaryType", SummaryType),
      GetRendererFromFormat("renderer"),
      IconLinkers,
      KV_Editor,
      KV_Plugins
      )
      : null;










    /// <summary>
    /// Идентификатор (имя) поля.
    /// </summary>
    [Required]
    public string Id => IsComplexProperty ? this["id"]?.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries)[0] : this["id"];

    /// <summary>
    /// Тип данных, хранящихся в этом поле.
    /// </summary>
    [Required]
    public string Type => this["type"] != "url" ? this["type"] : "string";
    
    /// <summary>
    /// Признак ключевого поля.
    /// По умолчанию = <see langword="false"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool? IsKey => parseBoolAttr("key");
    
    /// <summary>
    /// Опция группировки данных по этому полю.
    /// По умолчанию = <see langword="false"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool GroupBy => parseBoolAttr("group") ?? false;
    
    /// <summary>
    /// Тип аггрегатора, применяемого к этому полю.
    /// По умолчанию = <see langword="null"/>.
    /// </summary>
    public string SummaryType => this["summary"];
    
    /// <summary>
    /// Признак обязательного поля.
    /// По умолчанию = <see langword="false"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool? Required => parseBoolAttr("req");
    
    /// <summary>
    /// Признак включения этого поля в модель данных, передаваемых на сервер.
    /// По умолчанию = <see langword="false"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool? Persist => parseBoolAttr("persist");
    
    /// <summary>
    /// Опция, допускающая значения null в этом поле.
    /// По умолчанию = <see langword="false"/>.
    /// </summary>
    [DefaultValue(false)]
    public bool Nullable => parseBoolAttr("nullable") ?? false;

    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице
    /// задает возможность перетаскивания столбца внутри таблицы.
    /// </summary>
    public bool? Draggable => parseBoolAttr("draggable");

    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице
    /// задает возможность сортировки данных в столбце.
    /// </summary>
    public bool? Sortable => parseBoolAttr("sortable");
    
    /// <summary>
    /// Идентификатор/селектор реального поля в модели данных, 
    /// к которому привязано значение этого поля-дублера.
    /// </summary>
    public string Mapping => this["mapping"];
    
    /// <summary>
    /// Значение по умолчанию для этого поля.
    /// </summary>
    public string Default => this["default"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает его ширину в пикселях.
    /// </summary>
    public int? Width => parseIntAttr("width");
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает его минимальную ширину в пикселях.
    /// </summary>
    public int? MinWidth => parseIntAttr("minWidth");
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает его максимальную ширину в пикселях.
    /// </summary>
    public int? MaxWidth => parseIntAttr("maxWidth");
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает коэффициент гибкого растяжения столбца.
    /// </summary>
    public double? Flex => parseDoubleAttr("flex");
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает формат отображения данных столбца.
    /// </summary>
    public string Format => this["format"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает выравнивание содержимого столбца по горизонтали.
    /// </summary>
    [DefaultValue("none")]
    public string Align => this["align"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает текст всплывающей подсказки для заголовка столбца.
    /// </summary>
    public string Tooltip => this["tooltip"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает CSS-класс, применяемый к HTML-элементу td, сгенерированному для этого столбца.
    /// </summary>
    public string Cls => this["cls"];
    
    /// <summary>
    /// 
    /// </summary>
    public bool IsForRecordCls => parseBoolAttr("recordCls") ?? false;
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в дереве узлов 
    /// задает его тип как treecolumn.
    /// </summary>
    [DefaultValue(false)]
    public bool IsTree => parseBoolAttr("tree") ?? false;
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает тип инлайн-редактора для столбца.
    /// В случае, если соответствующий XML-атрибут не указан, редактирование
    /// значений для этого столбца будет недоступно.
    /// </summary>
    public string Editor
    {
      get
      {
        var ret = this["editor"];
        if (ret == "textfield" || ret == "text") ret = "TextField";
        if (ret == "textareafield" || ret == "textarea") ret = "TextArea";
        if (ret == "numberfield" || ret == "number") ret = "NumberField";
        if (ret == "numberset") ret = "NumberSet";
        if (ret == "datefield" || ret == "date") ret = "DateField";
        if (ret == "dateset") ret = "DateSet";
        if (ret == "timefield" || ret == "time") ret = "TimeField";
        if (ret == "combobox" || ret == "combo") ret = "combobox";
        if (ret == "selector") ret = "combobox";
        if (ret == "pairedcombo") ret = "PairedCombo";

        return ret;
      }
    }
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов,
    /// задает идентификатор элемента - инлайн-редактора, тип которого указан в атрибуте editor.
    /// </summary>
    public string EditorId => this["editorId"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов,
    /// при выбранном типе инлайн-редактора = selector или combobox,
    /// задает имя значимого поля в модели данных инлайн-комбобокса.
    /// В этом случае отображаемым полем в модели данных этого инлайн-комбобокса
    /// будет являться поле, имя которого указано в атрибуте id.
    /// </summary>
    public string EditorValueField => this["editorValueField"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает тип фильтра для значений в этом столбце.
    /// В случае, если соответствующий XML-атрибут не указан, в таблице/дереве узлов
    /// с подключенной опцией FiltersFeature тип фильтра для этого столбца
    /// будет определен и установлен автоматически.
    /// </summary>
    public string Filter => this["filter"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает источник (хранилище) данных для инлайн-комбобокса - 
    /// редактора значений в этом столбце. 
    /// Установка этого свойства имеет эффект только для <see cref="Field.Editor" />,
    /// равного "combobox".
    /// </summary>
    public string DataSrc => this["datasrc"];
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает строку-префикс, выводимую перед значением в этом столбце.
    /// </summary>
    public string Prefix => this["prefix"] ?? "''";
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает строку-суффикс, выводимую после значения в этом столбце.
    /// </summary>
    public string Suffix => this["suffix"] ?? "''";
    
    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// задает количество знаков после запятой для числовых значений в этом столбце.
    /// </summary>
    public int Precision => (this["precision"] != null) ? int.Parse(this["precision"]) : 0;
    
    /// <summary>
    /// Задает разделитель для одиночных значений, сцепленных в единую строку.
    /// Включает возможность чтения/записи множественных значений в одну ячейку/строку/поле.
    /// Включает возможность множественного выбора значений в инлайн-комбобоксе - 
    /// редакторе значений в столбце таблицы/дерева узлов.
    /// </summary>
    public string JoinBy => this["joinby"];




    /// <summary>
    /// При использовании этого поля в качестве столбца в таблице/дереве узлов 
    /// <see langword="true"/> указывает на запрет ввода пользовательских значений в инлайн-комбобокс 
    /// при редактировании значений в этом столбце.
    /// Для создания инлайн-комбобокса, позволяющего ввод новых вариантов выбора, XML-атрибут editor должен быть = "combobox".
    /// Для создания инлайн-комбобокса со строго ограниченным списком опций XML-атрибут editor должен быть = "selector".
    /// </summary>
    public bool StrictComboEdit => this["editor"] == "selector";

    public bool ForceSelection => Required == true;

    public bool IsComplexProperty => this["id"]?.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries).Length > 1;





    public IList<IPlugin> Plugins => _plugins;




    public List<Icon> Icons => _items.OfType<Icon>().ToList();
    public KeyValue IconLinkers => GetXTypeFromField() == "iconcol" && Icons != null ? KV("iconlinkers", Arr(false, true, Icons?.Select(s => s.AsExprs))) : null;





    public KeyValue KV_Plugins => Plugins.Count() > 0 ? KV("plugins", Arr(false, false, Plugins.Select(s => s.AsConfig))) : null;




    public KeyValue KV_Editor => Editor != null ? KV("editor", OBJ_Editor_Config) : null;

    private ObjectBody OBJ_Editor_Config =>
      Editor == "combobox"
      ?
      OBJ(false, false,
        KV("xtype", "combobox"),
        KV("editable", !StrictComboEdit),
        KV("queryMode", "local"),
        KV("valueField", EditorValueField ?? Id),
        KV("displayField", Id),
        KV("multiSelect", !string.IsNullOrWhiteSpace(JoinBy)),
        KV("forceSelection", ForceSelection),
        !string.IsNullOrWhiteSpace(JoinBy) ? KV("delimiter", JoinBy) : null
        )
      :
      Editor == "PairedCombo"
      ?
      OBJ(false, false,
        KV("xtype", Editor),
        KV("valueField", EditorValueField ?? Id),
        KV("displayField", Id),
        PROP("SourceId", DataSrc)
        )
      :
      OBJ(false, false, KV("xtype", Editor));





    public KeyValue KV_ItemId => this["id"] != null ? KV("name", Id) : null;
    public KeyValue KV_Text => PROP("text", Name);
    public KeyValue KV_DataIndex => PROP("dataIndex", Id);
    public KeyValue KV_Type => PROP("type", Type);
    public KeyValue KV_DateFormat => this["type"] == "date" || this["type"] == "time" ? KV("dateFormat", "c") : null;
    public KeyValue KV_Mapping => PROP("mapping", Mapping);
    public KeyValue KV_UseNull => Nullable == true ? KV("useNull", TRUE) : null;
    public KeyValue KV_Default => PROP("defaultValue", Default);
    public KeyValue KV_Persist => PROP("persist", Persist);
    public KeyValue KV_Convert => IsComplexProperty ? KV("convert", FN(Args(ID("val")), RETURN(MEMB(ID("val"), this["id"].Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray())))) : null;
    public KeyValue KV_Filterable => Filter == "none" ? KV("filterable", false) : null;
    public KeyValue KV_Draggable => PROP("draggable", Draggable);
    public KeyValue KV_Sortable => PROP("sortable", Sortable);




    public string GetXTypeFromField()
    {
      var ret = "gridcolumn";
      switch (this["type"])
      {
        case "auto":
        case "string":
        case "time":
        case "ID": ret = "gridcolumn"; break;
        case "url": ret = "picturecol"; break;
        case "int":
        case "float":
        case "double": ret = "numbercol"; break;
        case "boolean": ret = (Editor != null && Editor == "checkbox") ? "checkcolumn" : "checkcolumn"; break;
        case "date": ret = "datecol"; break;

        default: break;
      };

      if (Name != null && XMLSource.HasElement(x => x.Name.LocalName == "Icon"))
        ret = "iconcol";

      if (IsTree) ret = "treecolumn";

      //if (Editor == "combobox") ret = "ComboColumn";

      return ret;
    }
    
    

    [ToDo("Вынести создание JS-выражений в сервисы")]
    public KeyValue GetRendererFromFormat(string keyName)
    {
      switch (Format)
      {
        case "fileSize": return KV(keyName, FN(Args(ID("v")), RETURN(MEMB(ID("Ext"), ID("util"), ID("Format"), CALL("fileSize", Args(ID("v")))))));
        case "uppercase": return KV(keyName, FN(Args(ID("v")), RETURN(MEMB(ID("Ext"), ID("util"), ID("Format"), CALL("uppercase", Args(ID("v")))))));
        case "lowercase": return KV(keyName, FN(Args(ID("v")), RETURN(MEMB(ID("Ext"), ID("util"), ID("Format"), CALL("lowercase", Args(ID("v")))))));
        case "capitalize": return KV(keyName, FN(Args(ID("v")), RETURN(MEMB(ID("Ext"), ID("util"), ID("Format"), CALL("capitalize", Args(ID("v")))))));
        case "currency": return KV(keyName, FN(Args(ID("v")), RETURN(MEMB(ID("Ext"), ID("util"), ID("Format"), CALL("currency", Args(ID("v"), Str(Suffix), Num(Precision), TRUE))))));


        default: return null;
      };
    }

  }
}
