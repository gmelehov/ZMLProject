using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Линкер, связывающий внешнее состояние элемента с событием его внешнего контекста.
/// </summary>
public interface IContextBinder : IZmlComponent, IHasId, INeedSelection
{

  /// <summary>
  /// Идентификатор подключаемого элемента.
  /// Для случаев, когда этот линкер предназначен не для контекстного элемента (в котором он определен в разметке),
  /// а для другого, имеющего этот идентификатор. При этом настройка и добавление управляющего плагина 
  /// к такому элементу выполняются динамически, при инициализации контекстного элемента.
  /// </summary>
  string For { get; }

  /// <summary>
  /// Тип внешнего контекста, управляющего состоянием подключаемого элемента.
  /// Возможные значения см. в определении XSD-типа ContextScopes.
  /// </summary>
  string Scope { get; }

  /// <summary>
  /// Условие, которому должен соответствовать внешний контекст для активации линкера.
  /// Возможные значения см. в определении XSD-типа EnablingContexts.
  /// </summary>
  string If { get; }

  /// <summary>
  /// Управляющее событие внешнего контекста.
  /// </summary>
  string HostEvent { get; }







  KeyValue KV_For { get; }

  KeyValue KV_Scope { get; }

  KeyValue KV_If { get; }

}
