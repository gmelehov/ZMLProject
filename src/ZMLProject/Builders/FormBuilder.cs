using ZMLProject.Models.Forms;






namespace ZMLProject.Builders
{
  /// <summary>
  /// 
  /// </summary>
  public class FormBuilder : Builder<Form>
  {
    public FormBuilder(Form component) : base(component) { }








    public string XType => "Form";


    public bool? AutoLoad => Self.parseBoolAttr("autoload") ?? true;


    public double? ColumnWidth => Self.parseDoubleAttr("columnWidth") ?? 1;


    public string Width => Self["width"] ?? "100%";


    public string Height => Self.MinHeight != null || Self.MaxHeight != null ? Self["height"] : Self["height"] ?? "100%";


    public string Layout => Self["layout"] ?? "anchor";


    public int? BodyBorder => Self.parseIntAttr("bodyBorder") ?? 0;


  }
}
