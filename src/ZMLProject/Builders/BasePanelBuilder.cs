namespace ZMLProject.Builders;

/// <summary>
/// 
/// </summary>
public class BasePanelBuilder : Builder<BasePanel>
{
  public BasePanelBuilder(BasePanel component) : base(component) { }






  public bool SplitterHover => Self.parseBoolAttr("splitterHover") ?? true;

  public int SplitterSize => Self.parseIntAttr("splitterSize") ?? 6;

  public string TitleColor => Self["titleColor"] ?? "white";

  public string TitleBackColor => Self["titleBackColor"] ?? "darkblue";

  public string BodyBackColor => Self["bodyBackColor"] ?? "white";

}
