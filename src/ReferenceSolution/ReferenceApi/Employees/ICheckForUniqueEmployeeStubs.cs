
namespace ReferenceApi.Employees;

public interface ICheckForUniqueEmployeeStubs
{
    Task<bool> CheckUniqueAsync(string slug, CancellationToken token);
}