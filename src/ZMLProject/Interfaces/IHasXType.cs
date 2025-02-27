using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Элемент, имеющий «визуальный тип» XType.
/// </summary>
public interface IHasXType
{

  /// <summary>
  /// Визуальный тип, извлеченный из атрибутов элемента разметки.
  /// </summary>
  string XType { get; }




  /// <summary>
  /// Представление визуального типа в виде свойства JS-объекта-конфигуратора элемента.
  /// </summary>
  KeyValue KV_XType { get; }

}
