using ZMLProject.Enums;


namespace ZMLProject.Attrs;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class ManagedPropsAttribute : Attribute
{
  public ManagedPropsAttribute(PropAccess propAccess, params string[] props)
  {
    PropAccess = propAccess;
    Props = props;
  }










  public PropAccess PropAccess { get; }


  public string[] Props { get; }

}
