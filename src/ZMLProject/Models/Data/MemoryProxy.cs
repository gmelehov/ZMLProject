using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Memory-прокси.
  /// </summary>
  public class MemoryProxy : BaseProxy
  {
    public MemoryProxy(LocalSource source) : base(source)
    {

    }







    public override ObjectBody AsConfig => OBJ(false, false,
      KV_Type,
      KV_IdParam
      );








    /// <summary>
    /// Тип прокси.
    /// </summary>
    public override string Type => Source?.App.bsrv.MemoryProxy(this).Type;








    public override KeyValue KV_Type => KV("type", Type);
    
    public KeyValue KV_IdParam => Source?.Key != null ? KV("idParam", Source?.Key) : null;

  }
}
