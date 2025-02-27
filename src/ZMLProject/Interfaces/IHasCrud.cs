namespace ZMLProject.Interfaces;

/// <summary>
/// Интерфейс для хранилища данных, поддерживающих CRUD-операции.
/// </summary>
public interface IHasCrud
{

  /// <summary>
  /// Признак наличия поддержки CRUD-операций.
  /// </summary>
  bool Crud { get; }
}
