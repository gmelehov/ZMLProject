using DiffCode.JSGenerator;

using System.Diagnostics;

using static DiffCode.JSGenerator.Generator;






namespace ZMLProject.Models.Data
{
  /// <summary>
  /// REST-прокси.
  /// </summary>
  public class RestProxy : BaseProxy
  {
    public RestProxy(RestSource source) : base(source) { }
    public RestProxy(RestAction action) : base(action) { }






    public override ObjectBody AsConfig => OBJ(false, false,
      KV_Type,
      KV_IdParam,
      KV_Url,
      KV_Path
      );









    public override string Type => Source?.App.bsrv.RestProxy(this).Type ?? Action?.App.bsrv.RestProxy(this).Type;


    public string Url => Source?.As<RestSource>()?.BaseUrl ?? Action?.As<RestAction>()?.BaseUrl;
    

    public string Path => Source?.Name ?? Action?.Name;








    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public override KeyValue KV_Type => KV("type", Type);


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_IdParam => Source?.Key != null ? KV("idParam", Source.Key) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Url => Url != null ? KV("url", Url) : null;


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public KeyValue KV_Path => Path != null ? KV("path", Path) : null;

  }
}
