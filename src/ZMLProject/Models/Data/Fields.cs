using DiffCode.JSGenerator;
using static DiffCode.JSGenerator.Generator;
using System.Xml.Linq;





namespace ZMLProject.Models.Data
{
  /// <summary>
  /// Элемент интерфейса:
  /// группа столбцов таблицы данных.
  /// </summary>
  public class Fields : BaseField
  {
    public Fields(IZmlContainer host, XElement source) : base(host, source) => _items.AddRange(source.Elements(F("Field")).Select(s => new Field(this, s)));






    public override ObjectBody AsConfig => null;





    public override ObjectBody Column => OBJ();

  }
}
