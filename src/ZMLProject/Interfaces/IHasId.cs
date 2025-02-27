using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Элемент, имеющий идентификатор.
/// </summary>
public interface IHasId
{

  /// <summary>
  /// Значение идентификатора, извлеченное из атрибутов элемента разметки.
  /// </summary>
  string Id { get; }


  /// <summary>
  /// Представление идентификатора в виде свойства JS-объекта-конфигуратора элемента.
  /// </summary>
  KeyValue KV_ItemId { get; }

}
