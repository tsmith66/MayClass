
namespace ReferenceApi.Employees;

public interface IGenerateSlugsForNewEmployees
{
    Task<string> GenerateAsync(string firstName, string? lastName, CancellationToken token = default);
}