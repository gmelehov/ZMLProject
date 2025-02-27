using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Элемент, имеющий базовые визуальные размеры
/// (поля, отступы, ширину, высоту).
/// </summary>
public interface IHasBaseRectSizes
{

  /// <summary>
  /// Ширина.
  /// </summary>
  string Width { get; }

  /// <summary>
  /// Высота.
  /// </summary>
  string Height { get; }

  /// <summary>
  /// Поля.
  /// </summary>
  string Margin { get; }

  /// <summary>
  /// Отступы.
  /// </summary>
  string Padding { get; }







  KeyValue KV_Width { get; }

  KeyValue KV_Height { get; }

  KeyValue KV_Margin { get; }

  KeyValue KV_Padding { get; }

}
