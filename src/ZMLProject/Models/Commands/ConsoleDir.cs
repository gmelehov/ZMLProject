using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Models.Valuables;
using DiffCode.CommonExtensions;





namespace ZMLProject.Models.Commands
{
  /// <summary>
  /// Команда console.dir(...)
  /// </summary>
  public class ConsoleDir : BaseCommand
  {
    public ConsoleDir(IZmlContainer host, XElement source) : base(host, source)
    {
      var elems = source.Elements().ToList();
      elems.ForEach(b =>
      {
        ZmlComponent elem = null;
        switch (b.Name.LocalName)
        {
          case "get.text": elem = new GetText(this, b); break;
          case "get.number": elem = new GetNumber(this, b); break;
          case "get.prop": elem = new GetProp(this, b); break;


          default: break;
        };
        if (elem != null) _items.Add(elem);
      });
    }







    /// <summary>
    /// Представление компонента в виде набора JavaScript-выражений.
    /// </summary>
    public override AbstractExpression AsExprs
    {
      get
      {
        if (Text.IsUseful())
        {
          return App.jsrv.Console_Dir(Str(Text));
        }
        else if (Values.Count() > 0)
        {
          return Values.Count() == 1 ? App.jsrv.Console_Dir(Values.FirstOrDefault()) : App.jsrv.Console_Dir(Arr(Values));
        }
        else
        {
          return null;
        };
      }
    }








    /// <summary>
    /// Текст, отображаемый в консоли браузера при запуске этой команды.
    /// </summary>
    public string Text => this["text"];





    /// <summary>
    /// Коллекция JavaScript-выражений из дочерних компонентов.
    /// </summary>
    public IEnumerable<AbstractExpression> Values => _items.OfType<BaseValuable>()?.Select(s => s.AsExprs);

  }
}
