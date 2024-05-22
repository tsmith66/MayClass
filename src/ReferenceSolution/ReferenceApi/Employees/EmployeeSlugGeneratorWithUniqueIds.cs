
namespace ReferenceApi.Employees;

public class EmployeeSlugGeneratorWithUniqueIds(ICheckForUniqueEmployeeStubs uniquenessChecker) : IGenerateSlugsForNewEmployees
{
    public async Task<string> GenerateAsync(string firstName, string? lastName, CancellationToken token = default)
    {
        var slug = (Clean(firstName), Clean(lastName)) switch
        {
            (string first, null) => first,
            (string first, string last) => $"{last}-{first}",
            _ => throw new InvalidOperationException() // Chaos
        };

        bool isUnique = await uniquenessChecker.CheckUniqueAsync(slug, token);
        if (isUnique)
        {
            return slug;
        }

        var letters = "abcdefghijklmnopqrstuvwxyz".Select(c => c).ToList();
        foreach (var letter in letters)
        {
            var attempt = slug + '-' + letter;
            if (await uniquenessChecker.CheckUniqueAsync(attempt, token))
            {
                return attempt;
            }
        }

        return SlugWithUniqueIdentifier(slug);
    }
    private static string? Clean(string? part)
    {
        if (string.IsNullOrWhiteSpace(part))
        {
            return null;
        }
        return part.ToLowerInvariant().Trim();
    }

    private static string SlugWithUniqueIdentifier(string slug)
    {
        var base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        base64Guid.Replace('/', '_');
        base64Guid.Replace('+', '_');
        return slug + base64Guid[..22];
    }
}
