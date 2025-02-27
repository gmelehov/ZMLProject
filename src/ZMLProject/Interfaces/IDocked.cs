using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Контейнер для пристыкованных дочерних элементов.
/// </summary>
public interface IDocked : IZmlContainer
{

  /// <summary>
  /// Список пристыкованных форм.
  /// </summary>
  IEnumerable<IForm> Forms { get; }

  /// <summary>
  /// Список пристыкованных тулбаров.
  /// </summary>
  IEnumerable<IToolbar> Toolbars { get; }

  /// <summary>
  /// Список пристыкованных кнопок.
  /// </summary>
  IEnumerable<IButton> Buttons { get; }







  KeyValue KV_DockedItems { get; }
}
