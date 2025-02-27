using DiffCode.JSGenerator;

using System.Xml.Linq;


namespace ZMLProject.Interfaces;

/// <summary>
/// Базовый элемент интерфейса.
/// </summary>
public interface IZmlComponent
{

  string this[string str] { get; }
  IZmlContainer Host { get; }
  XElement XMLSource { get; }







  T As<T>() where T : class, IZmlComponent;
  IList<T> Find<T>() where T : class, IZmlComponent;
  IList<T> Find<T>(Func<T, bool> predicate) where T : class, IZmlComponent;
  THost Closest<THost>() where THost : class, IZmlContainer;






  ExtJSClassExpression AsDefine { get; }
  ObjectBody AsConfig { get; }






  bool? parseBoolAttr(string xname);
  int? parseIntAttr(string xname);
  double? parseDoubleAttr(string xname);
  KeyValue PROP(string xname, object val);

}
