namespace ZMLProject.Interfaces;

/// <summary>
/// Объект, поддерживающий привязку своего состояния к внешнему контексту.
/// </summary>
public interface IHasContextBinders : IHasPlugins
{

  /// <summary>
  /// Список линкеров, связывающих элемент с событием внешнего контекста.
  /// </summary>
  IEnumerable<IContextBinder> ContextBinders { get; }

}
