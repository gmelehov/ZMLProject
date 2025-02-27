using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

///// <summary>
///// 
///// </summary>
//public interface IFormItem : IZMLContainer, ILabelable, IHasVisualSizes
//{

//  /// <summary>
//  /// 
//  /// </summary>
//  bool? Disabled { get; }



//  KeyValue KV_Disabled { get; }
//}







/// <summary>
/// 
/// </summary>
public interface IFormItem : IZmlContainer, ILabelable, IHasVisualSizes
{

  bool? AllowBlank { get; }

  string Anchor { get; }

  string Cls { get; }

  double? ColumnWidth { get; }

  bool? Disabled { get; }








  KeyValue KV_AllowBlank { get; }
  KeyValue KV_Anchor { get; }
  KeyValue KV_Cls { get; }
  KeyValue KV_ColumnWidth { get; }
  KeyValue KV_Disabled { get; }
}
