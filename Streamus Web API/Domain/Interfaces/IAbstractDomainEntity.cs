namespace Streamus_Web_API.Domain.Interfaces
{
  public interface IAbstractDomainEntity<T>
  {
    T Id { get; set; }
  }
}