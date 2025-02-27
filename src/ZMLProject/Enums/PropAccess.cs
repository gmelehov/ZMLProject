namespace ZMLProject.Enums;

/// <summary>
/// Варианты использования управляемых свойств компонентов.
/// </summary>
public enum PropAccess
{

  /// <summary>
  /// Чтение управляемого свойства.
  /// </summary>
  Getter = 1,

  /// <summary>
  /// Установка управляемого свойства.
  /// </summary>
  Setter,

  /// <summary>
  /// Добавление значения к управляемому свойству.
  /// </summary>
  ValueAdder,

  /// <summary>
  /// Удаление значения из управляемого свойства.
  /// </summary>
  ValueRemover,

  /// <summary>
  /// Вызов метода.
  /// </summary>
  Caller

}
