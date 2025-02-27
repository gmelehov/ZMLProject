namespace ZMLProject.Interfaces;

/// <summary>
/// Тулбар.
/// </summary>
public interface IToolbar : IZmlContainer, IHasXType, IDockable
{

  /// <summary>
  /// Список пристыкованных кнопок.
  /// </summary>
  IEnumerable<IButton> Buttons { get; }

}
