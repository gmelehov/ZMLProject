using System.Diagnostics;
using System.Xml.Linq;



namespace ZMLProject.Models;

/// <summary>
/// Базовый класс для контейнеров.
/// </summary>
public abstract class ZmlContainer : ZmlComponent, IZmlContainer
{
  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected IDocked _docked;

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  protected List<IZmlComponent> _items = new List<IZmlComponent>();






  protected ZmlContainer(XElement source) : base(source) { }
  protected ZmlContainer(IZmlContainer host, XElement source) : base(host, source) { }






  public IList<IZmlComponent> Items => _items;
  public IDocked Docked => _docked;








  public override IList<TOut> Find<TOut>(Func<TOut, bool> predicate)
  {
    var ret = new List<TOut>();
    if (As<TOut>() != null && predicate(As<TOut>())) ret.Add(As<TOut>());
    ret.AddRange(Items.Where(w => w != null).SelectMany(ss => ss?.Find(predicate)));
    if (Docked != null) ret.AddRange(Docked?.Items?.Where(w => w != null)?.SelectMany(ss => ss?.Find(predicate)));
    return ret;
  }
  public override IList<TOut> Find<TOut>()
  {
    var ret = new List<TOut>();
    if (As<TOut>() != null) ret.Add(As<TOut>());
    ret.AddRange(Items.Where(w => w != null).SelectMany(ss => ss?.Find<TOut>()));
    if (Docked != null) ret.AddRange(Docked?.Items?.Where(w => w != null)?.SelectMany(ss => ss?.Find<TOut>()));
    return ret;
  }

}
