using ZMLProject.Models.Forms;








namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class ComboboxBuilder : Builder<Combobox>
  {
    public ComboboxBuilder(Combobox component) : base(component) { }







    public string XType => "ComboBox";

  }
}
