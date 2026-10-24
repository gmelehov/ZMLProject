using ZMLProject;
using ZMLProject.Services;



var builder = new BuilderService();
var jsexpr = new JSExpressionsService();

var app = Utils.CompileApplication(@"F:\ZMLProject\src\ConsApp\Test2.xml", builder, jsexpr);

var appjs = app.AsAppJSContent;













Console.WriteLine("Hello, World!");
Console.WriteLine("Hello, World!");
