using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class CollectionBuilder : Builder<Collection>
  {
    public CollectionBuilder(Collection component) : base(component) { }







    public string XType => "Collection";

  }
}
