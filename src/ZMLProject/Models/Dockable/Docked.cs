using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using ZMLProject.Models.Forms;
using System.Diagnostics;






namespace ZMLProject.Models.Dockable
{
  /// <summary>
  /// Контейнер для пристыкованных дочерних элементов.
  /// </summary>
  public class Docked : ZmlContainer, IDocked
  {
    public Docked(IZmlContainer host, XElement source) : base(host, source)
    {
      _items.AddRange(source.Elements().Select(el =>
      {
        var name = el.Name.LocalName;
        ZmlComponent ret;
        switch (name)
        {
          case "Toolbar": ret = new Toolbar(this, el); break;
          case "Form": ret = new Form(this, el); break;
          case "Panel": ret = new Panel(this, el); break;

          default: ret = null; break;
        };
        return ret;
      }).Where(w => w != null));
    }







    public override ExtJSClassExpression AsDefine => null;
    public override ObjectBody AsConfig => OBJ(false, true,
      KV_DockedItems
      );





    
    
    public IEnumerable<IForm> Forms => Items?.OfType<IForm>();
    public IEnumerable<IToolbar> Toolbars => Items?.OfType<IToolbar>();
    public IEnumerable<IButton> Buttons => Toolbars?.SelectMany(s => s?.Buttons);






    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_DockedItems => KV("dockedItems", Arr(false, true, Items?.Select(s => s.AsConfig)));
  }
}
