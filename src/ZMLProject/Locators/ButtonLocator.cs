using ZMLProject.Models.Dockable;







namespace ZMLProject.Locators
{
  /// <summary>
  /// 
  /// </summary>
  public class ButtonLocator : Locator<Button>
  {
    public ButtonLocator(string glob, string app, string id) : base(glob, app, id) { }










    public override string Manager => "Button";
  }
}
