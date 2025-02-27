using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Элемент, имеющий расширенное определение визуальных размеров
/// (флекс, мин/макс ширины и высоты).
/// </summary>
public interface IHasVisualSizes : IHasBaseRectSizes
{

  /// <summary>
  /// Флекс.
  /// </summary>
  double? Flex { get; }

  /// <summary>
  /// Минимальная ширина.
  /// </summary>
  string MinWidth { get; }

  /// <summary>
  /// Максимальная ширина.
  /// </summary>
  string MaxWidth { get; }

  /// <summary>
  /// Минимальная высота.
  /// </summary>
  string MinHeight { get; }

  /// <summary>
  /// Максимальная высота.
  /// </summary>
  string MaxHeight { get; }






  KeyValue KV_Flex { get; }

  KeyValue KV_MinWidth { get; }

  KeyValue KV_MaxWidth { get; }

  KeyValue KV_MinHeight { get; }

  KeyValue KV_MaxHeight { get; }

}
