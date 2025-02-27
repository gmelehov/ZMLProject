using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Кнопка.
/// </summary>
public interface IButton : IPluginHost, IHasXType, INeedSelection
{



  string Call { get; }


  string TargetId { get; }


  string Text { get; }


  string Icon { get; }


  string IconSet { get; }


  string Color { get; }


  string Align { get; }


  string Margin { get; }


  string Padding { get; }


  double? ColumnWidth { get; }


  bool Disabled { get; }


  string AfterClick { get; }











  KeyValue KV_Call { get; }

  KeyValue KV_TargetId { get; }

  KeyValue KV_IconSet { get; }

  KeyValue KV_Text { get; }

  KeyValue KV_Align { get; }

  KeyValue KV_Icon { get; }

  KeyValue KV_Color { get; }

  KeyValue KV_Margin { get; }

  KeyValue KV_Padding { get; }

  KeyValue KV_ColumnWidth { get; }

  KeyValue KV_Disabled { get; }

}
