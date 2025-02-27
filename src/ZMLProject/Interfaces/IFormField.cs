using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IFormField : IFormItem, IHasId, IHasXType, IHasAnchor, IHasAutoSync, INeedSelection, IHasColumnWidth
{

  /// <summary>
  /// 
  /// </summary>
  bool? ReadOnly { get; }

  /// <summary>
  /// 
  /// </summary>
  string FieldLabel { get; }

  /// <summary>
  /// 
  /// </summary>
  string AltName { get; }



  /// <summary>
  /// Признак того, что у этого элемента есть соседи по родительскому диалогу/форме, являющиеся группой полей ввода данных.
  /// Признак используется для автоматического расчета полей этого элемента внутри родительского элемента.
  /// </summary>
  bool HasFieldSetSiblings { get; }

  /// <summary>
  /// Признак того, что следующий за этим элементом элемент является группой полей ввода данных.
  /// Признак используется для автоматического расчета полей этого элемента внутри родительского элемента.
  /// </summary>
  bool NextIsFieldSet { get; }










  KeyValue KV_ReadOnly { get; }


  KeyValue KV_FieldLabel { get; }


  KeyValue KV_AltName { get; }
}
