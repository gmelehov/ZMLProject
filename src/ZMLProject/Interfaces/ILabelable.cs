using DiffCode.JSGenerator;


namespace ZMLProject.Interfaces;

/// <summary>
/// Объект, имеющий метку.
/// </summary>
public interface ILabelable
{


  string LabelAlign { get; }

  int? LabelWidth { get; }

  string LabelClsExtra { get; }






  KeyValue KV_LabelAlign { get; }
  
  KeyValue KV_LabelWidth { get; }
  
  KeyValue KV_LabelClsExtra { get; }

}
