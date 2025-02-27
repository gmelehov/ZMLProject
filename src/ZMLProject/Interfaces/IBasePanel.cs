using DiffCode.JSGenerator;

using System.Xml.Linq;



namespace ZMLProject.Interfaces;

/// <summary>
/// Базовая панель.
/// </summary>
public interface IBasePanel : IZmlContainer, IHasId, IHasXType, IHasVisualSizes, IPluginHost, IHasAnchor
{


  /// <summary>
  /// Обработка отсутствующих в разметке наиболее употребительных атрибутов.
  /// </summary>
  void ProcessCommonAttributes();

  /// <summary>
  /// Обработка атрибутов fixWidth, fixHeight.
  /// </summary>
  void ProcessFixedSizes();

  /// <summary>
  /// Обработка дочерних элементов, пристыкованных к этой панели.
  /// </summary>
  /// <param name="source"></param>
  void ProcessDockedItems(XElement source);

  /// <summary>
  /// Автоматическое добавление сплиттеров между дочерними контейнерами.
  /// </summary>
  /// <param name="source"></param>
  /// <returns></returns>
  XElement AutoAddSplitters(XElement source);

  /// <summary>
  /// Обработка вложенных дочерних элементов.
  /// </summary>
  /// <param name="source"></param>
  void ProcessCommonElements(XElement source);















  string Title { get; }


  object Header { get; }


  string BodyCls { get; }


  string Cls { get; }


  bool? Disabled { get; }


  string OverflowX { get; }


  string OverflowY { get; }


  string Region { get; }


  int? Border { get; }


  int? BodyBorder { get; }


  string BodyPadding { get; }


  string CollapseTo { get; }


  bool? Collapsible { get; }


  bool? TitleCollapse { get; }


  bool? Split { get; }


  bool SplitterHover { get; }


  int SplitterSize { get; }


  string Layout { get; }


  string TitleColor { get; }


  string TitleBackColor { get; }


  string BodyBackColor { get; }










  KeyValue KV_Title { get; }


  KeyValue KV_Header { get; }


  KeyValue KV_BodyCls { get; }


  KeyValue KV_Cls { get; }


  KeyValue KV_Disabled { get; }


  KeyValue KV_OverflowX { get; }


  KeyValue KV_OverflowY { get; }


  KeyValue KV_Region { get; }


  KeyValue KV_Border { get; }


  KeyValue KV_BodyBorder { get; }


  KeyValue KV_BodyPadding { get; }


  KeyValue KV_CollapseTo { get; }


  KeyValue KV_Collapsible { get; }


  KeyValue KV_TitleCollapse { get; }


  KeyValue KV_Split { get; }


  KeyValue KV_Layout { get; }

}
