using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Элемент, поддерживающий подключение плагинов.
/// </summary>
public interface IHasPlugins : IZmlContainer, IHasId
{

  /// <summary>
  /// Список подключенных к элементу плагинов.
  /// </summary>
  IList<IPlugin> Plugins { get; }





  /// <summary>
  /// Представление списка плагинов в виде свойства JS-объекта-конфигуратора элемента.
  /// </summary>
  KeyValue KV_Plugins { get; }

}
