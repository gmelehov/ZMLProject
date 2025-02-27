namespace ZMLProject.Attrs
{
  /// <summary>
  /// Дескриптор события, определяемого в разметке.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
  public class EventDescriptorAttribute : Attribute
  {
    public EventDescriptorAttribute(string jsMethod, string xmlAttr)
    {
      JSMethod = jsMethod;
      XmlAttr = xmlAttr;
    }







    /// <summary>
    /// Наименование JavaScript-метода, обрабатывающего событие.
    /// Задается значением XML-атрибута.
    /// </summary>
    public string JSMethod { get; }

    /// <summary>
    /// Тип события.
    /// Задается именем XML-атрибута.
    /// </summary>
    public string XmlAttr { get; }

  }
}
