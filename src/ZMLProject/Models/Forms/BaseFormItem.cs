using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using System.Diagnostics;







namespace ZMLProject.Models.Forms
{
  /// <summary>
  /// Базовый элемент формы ввода данных 
  /// (поля формы, наборы полей формы, настройки полей по умолчанию).
  /// </summary>
  public abstract class BaseFormItem : ZmlContainer, IFormItem
  {
    protected BaseFormItem(IZmlContainer host, XElement source) : base(host, source) { }








    public override ExtJSClassExpression AsDefine => null;











    public virtual bool? AllowBlank => parseBoolAttr("allowBlank");


    public virtual string Anchor => this["anchor"];


    public virtual string Cls => this["cls"];


    public virtual double? ColumnWidth => parseDoubleAttr("columnWidth");


    public virtual bool? Disabled => parseBoolAttr("disabled");


    public virtual double? Flex => parseDoubleAttr("flex");


    public virtual string Height => this["height"];


    public virtual string LabelAlign => this["labelAlign"];


    public virtual string LabelClsExtra => this["labelClsExtra"];


    public virtual int? LabelWidth => parseIntAttr("labelWidth");


    public virtual string Margin => this["margin"];


    public virtual string MaxHeight => this["maxHeight"];


    public virtual string MaxWidth => this["maxWidth"];


    public virtual string MinHeight => this["minHeight"];


    public virtual string MinWidth => this["minWidth"];


    public virtual string Padding => this["padding"];


    public virtual string Width => this["width"];













    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_AllowBlank => PROP("allowBlank", AllowBlank);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Anchor => PROP("anchor", Anchor);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Cls => PROP("cls", Cls);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_ColumnWidth => PROP("columnWidth", ColumnWidth);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Disabled => PROP("disabled", Disabled);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Flex => PROP("flex", Flex);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Height => PROP("height", Height);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_LabelAlign => PROP("labelAlign", LabelAlign);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_LabelClsExtra => AllowBlank != null && LabelClsExtra != null ? KV("labelClsExtra", LabelClsExtra) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_LabelWidth => PROP("labelWidth", LabelWidth);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Margin => PROP("margin", Margin);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_MaxWidth => PROP("maxWidth", MaxWidth);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_MinWidth => PROP("minWidth", MinWidth);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_MaxHeight => PROP("maxHeight", MaxHeight);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_MinHeight => PROP("minHeight", MinHeight);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Padding => PROP("padding", Padding);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public virtual KeyValue KV_Width => PROP("width", Width);

  }
}
