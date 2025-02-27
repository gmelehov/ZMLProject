using DiffCode.JSGenerator;



namespace ZMLProject.Interfaces;

/// <summary>
/// Возможность пристыковки элемента интерфейса к сторонам родительского контейнера.
/// </summary>
public interface IDockable
{

  /// <summary>
  /// Сторона родительского контейнера.
  /// </summary>
  string Dock { get; }




  KeyValue KV_Dock { get; }

}
