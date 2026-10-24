using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;
using DiffCode.CommonExtensions;
using HandlebarsDotNet;
using ZMLProject.Services;
using DiffCode.JSGenerator;


namespace ZMLProject;

/// <summary>
/// 
/// </summary>
public static class Utils
{

  /// <summary>
  /// Возвращает экземпляр приложения, скомпилированный из XML-файла, находящегося по указанному пути.
  /// </summary>
  /// <param name="filepath">Путь к XML-файлу.</param>
  /// <returns></returns>
  public static ZmlApplication CompileApplication(string filepath, IBuilderService builder, IJSExpressionsService jsexpr)
  {
    var filecontent = XDocument.Load(filepath);
    if(filecontent != null)
    {
      AbstractToken.SetDoubleSpaceIndent();
      CompileItemTemplates();

      var app = new ZmlApplication(builder, jsexpr, filecontent.Root);
      return app;
    }
    else
    {
      return null;
    };
  }



  public static string CompileScripts(string filepath)
  {
    var compiler = XDocument.Load(filepath);
    //var appvar = ls.AppGlobalPath;
    //var appns = ls.LibClassesNamespace;
    var folders = compiler.Root.Elements(w => w.Name.LocalName == "CompileFolder" && w.HasElement(x => x.Name.LocalName == "Script"));
    var templates = folders
      .GroupBy(g => g.AttrVal("path"))
      .ToDictionary(
          k => k.Key,
          k => k.SelectMany(ss => ss.Elements(x => x.Name.LocalName == "Script"))
                .Select(s => Handlebars.Compile(File.ReadAllText(k.Key + s.AttrVal("src"))).Invoke(new {  })
                ));
    

    return string.Join("\r\n\r\n\r\n\r\n", templates.SelectMany(s => s.Value));
  }
  
  
  public static void CompileItemTemplates(string folderPath)
  {
    ItemTemplatesDict = new DirectoryInfo(folderPath).GetFiles("*.*", SearchOption.AllDirectories).ToDictionary(k => k.Name.Replace(k.Extension, ""), d => Handlebars.Compile(File.ReadAllText(d.FullName))).ToDictionary(kk => kk.Key, vv => vv.Value);
  }


  public static void CompileItemTemplates()
  {
    var templates = new Dictionary<string, string>
    {
      ["AggregatedCombo"] = Properties.Resources.AggregatedCombo,
      ["AggregatedList"] = ZMLProject.Properties.Resources.AggregatedList,
      ["BaseComboList"] = Properties.Resources.BaseComboList,
      ["ColorComboList"] = Properties.Resources.ColorComboList,
      ["ExtendedComboOneRow"] = Properties.Resources.ExtendedComboOneRow,
      ["ExtendedComboTwoRows"] = Properties.Resources.ExtendedComboTwoRows,
      ["NavigationList"] = Properties.Resources.NavigationList,
      ["SimpleList"] = Properties.Resources.SimpleList
    };
    ItemTemplatesDict = templates.ToDictionary(k => k.Key, d => Handlebars.Compile(d.Value));
  }



  public static Dictionary<string, HandlebarsTemplate<object, object>> ItemTemplatesDict { get; set; }

  public static List<string> CompileItemTemplateAsStrings(HandlebarsTemplate<object, object> templ, Dictionary<string, string> dict)
  {
    var data = new
    {
      itemSelector = dict.ContainsKey("itemSelector") ? "" + dict["itemSelector"] + "" : "",
      field_a = dict.ContainsKey("field_a") ? "{" + dict["field_a"] + "}" : "",
      field_b = dict.ContainsKey("field_b") ? "{" + dict["field_b"] + "}" : "",
      field_c = dict.ContainsKey("field_c") ? "{" + dict["field_c"] + "}" : "",
      field_d = dict.ContainsKey("field_d") ? "{" + dict["field_d"] + "}" : "",
      field_e = dict.ContainsKey("field_e") ? "{" + dict["field_e"] + "}" : "",
      field_f = dict.ContainsKey("field_f") ? "{" + dict["field_f"] + "}" : "",
      field_g = dict.ContainsKey("field_g") ? "{" + dict["field_g"] + "}" : "",
      field_h = dict.ContainsKey("field_h") ? "{" + dict["field_h"] + "}" : "",
    };
    var ret = templ(data);
    var tempFile = Path.GetTempFileName();
    File.WriteAllText(tempFile, ret);
    var content = File.ReadAllLines(tempFile);
    File.Delete(tempFile);
    return content.Select(s => s.Trim()).Where(w => w.Length > 0).ToList();
  }


  public static AbstractExpression GetNewExtTemplate(string templateName, Dictionary<string, string> templateDict) => new NewExpr(MEMB(ID("Ext"), CALL("XTemplate", Args(CompileItemTemplateAsStrings(ItemTemplatesDict[templateName], templateDict).Select(t => Str(t))))));



  public static MemberAccess MatIconLinker(string ns, ObjectBody obj) => MEMB(ID(ns), CALL("MatIconLinker", Args(obj)));
  public static MemberAccess IconLinker(string ns, ObjectBody obj) => MEMB(ID(ns), CALL("IconLinker", Args(obj)));



  public static XDocument GetEmptyXml()
  {
    var xdoc = new XDocument(
      new XElement(
        F("App"), 
        new XAttribute("globalVar", "GLOBAL"), 
        new XAttribute("name", "APP"),
        new XElement(F("Sources")),
        new XElement(F("Actions")),
        new XElement(F("ViewPort"))
        )
      );
    return xdoc;
  }


  public static XName F(string name) => XName.Get(name, "ZMLAppSchema");

}
