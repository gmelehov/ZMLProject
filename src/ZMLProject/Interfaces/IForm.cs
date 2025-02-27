using DiffCode.JSGenerator;



namespace ZMLProject.Interfaces;

/// <summary>
/// Форма ввода данных.
/// </summary>
public interface IForm : IBasePanel, IHasColumnWidth, IDockable
{


  /// <summary>
  /// 
  /// </summary>
  bool? AutoLoad { get; }

  /// <summary>
  /// 
  /// </summary>
  string Src { get; }

  /// <summary>
  /// 
  /// </summary>
  string OnValid { get; }

  /// <summary>
  /// Признак того, что в этой форме имеются группы полей ввода.
  /// </summary>
  bool HasFieldSets { get; }

  /// <summary>
  /// Список кнопок, расположенных в этой форме.
  /// </summary>
  IEnumerable<IButton> Buttons { get; }









  KeyValue KV_AutoLoad { get; }


  KeyValue KV_Src { get; }


  KeyValue KV_FieldDefaults { get; }


  KeyValue KV_OnValid { get; }

}
