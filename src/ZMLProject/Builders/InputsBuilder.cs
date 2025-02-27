using ZMLProject.Models.Forms;





namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class InputsBuilder : Builder<Inputs>
  {
    public InputsBuilder(Inputs component) : base(component) { }







    public string XType => "fieldset";

    public string Cls => "mat-icons";

    public string Layout => Self["layout"] ?? "anchor";

    public bool Collapsible => Self.parseBoolAttr("collapsible") ?? true;

  }
}
