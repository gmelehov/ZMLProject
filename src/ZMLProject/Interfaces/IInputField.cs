using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Поле ввода.
/// </summary>
public interface IInputField : IFormField, IHasContextBinders, IPluginHost
{


  string FieldName { get; }


  string Format { get; }


  string WriteFormat { get; }


  double? Step { get; }


  int? Increment { get; }


  int? DecimalPrecision { get; }


  string StartDay { get; }


  bool? Checked { get; }


  string InputValue { get; }


  int? Rows { get; }


  bool? ConvertQuotes { get; }


  string EmptyText { get; }


  string Value { get; }


  double? MinValue { get; }


  double? MaxValue { get; }


  bool SubmitEmptyText { get; }


  string FieldCls { get; }


  string MsgTarget { get; }


  string Delimiter { get; }

  /// <summary>
  /// Тип данных для поля ввода значения в элементе KeyValueInput
  /// </summary>
  string ValueType { get; }










  KeyValue KV_FieldName { get; }

  KeyValue KV_Format { get; }

  KeyValue KV_WriteFormat { get; }

  KeyValue KV_Step { get; }

  KeyValue KV_Increment { get; }

  KeyValue KV_DecimalPrecision { get; }

  KeyValue KV_StartDay { get; }

  KeyValue KV_Checked { get; }

  KeyValue KV_InputValue { get; }

  KeyValue KV_Rows { get; }

  KeyValue KV_ConvertQuotes { get; }

  KeyValue KV_EmptyText { get; }

  KeyValue KV_Val { get; }

  KeyValue KV_MinValue { get; }

  KeyValue KV_MaxValue { get; }

  KeyValue KV_SubmitEmptyText { get; }

  KeyValue KV_FieldCls { get; }

  KeyValue KV_MsgTarget { get; }

  KeyValue KV_Delimiter { get; }

  KeyValue KV_ValueType { get; }

}
