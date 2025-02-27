namespace ZMLProject.Interfaces;

/// <summary>
/// Базовый контейнер.
/// </summary>
public interface IZmlContainer : IZmlComponent
{

  /// <summary>
  /// Список дочерних (вложенных/зависимых/контекстных) элементов.
  /// </summary>
  IList<IZmlComponent> Items { get; }

  /// <summary>
  /// 
  /// </summary>
  IDocked Docked { get; }

}
