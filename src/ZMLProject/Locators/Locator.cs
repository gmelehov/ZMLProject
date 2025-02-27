using DiffCode.JSGenerator;

using static DiffCode.JSGenerator.Generator;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class Locator<T> : ILocatable where T : IZmlComponent
  {
    protected string _global;
    protected string _app;





    protected Locator(string glob, string app)
    {
      _global = glob;
      _app = app;
    }
    protected Locator(string glob, string app, string id) : this(glob, app)
    {
      Id = id;
    }







    public virtual MemberAccess Self() => new MemberAccess(ID(_global), ID(_app), ID(Manager), ID(Id));



    public virtual MemberAccess MGR() => new MemberAccess(ID(_global), ID(_app), ID(Manager));






    public string Id { get; }

    public abstract string Manager { get; }








    public override string ToString() => Self()?.Formatted;

  }
}
