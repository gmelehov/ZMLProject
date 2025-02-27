namespace ZMLProject.Interfaces;

/// <summary>
/// Интерфейс компонента, имеющего сгенерированную глобальную ссылку на самого себя
/// и обрабатываемого специальным менеджером компонентов.
/// </summary>
public interface ILocatable
{


  /// <summary>
  /// Идентификатор компонента.
  /// </summary>
  string Id { get; }


  /// <summary>
  /// Идентификатор менеджера.
  /// </summary>
  string Manager { get; }

}
