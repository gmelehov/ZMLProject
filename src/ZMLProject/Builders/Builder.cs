namespace ZMLProject.Builders;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Builder<T> where T : IZmlComponent
{
  protected Builder(T component) => Self = component;





  public T Self { get; }

}
