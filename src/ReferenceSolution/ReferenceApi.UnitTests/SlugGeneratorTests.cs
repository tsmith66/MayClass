using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class SlugGeneratorTests
{

    [Theory]
    [InlineData("Boba", "Fett", "fett-boba")]
    [InlineData("Luke", "Skywalker", "skywalker-luke")]
    [InlineData("Joe", "", "joe")]
    [InlineData("Cher", "", "cher")]
    [InlineData(" Joe", "Von Schmidt  ", "von_schmidt-joe", Skip = "Waiting")]
    [InlineData("Johnny", "Marr", "marr-johnny")]
    public async Task GeneratingSlugsForPostToEmployees(string firstName, string lastName, string expected)
    {
        // Given
        var slugGenerator = new EmployeeSlugGenerator();


        // When
        string slug = await slugGenerator.GenerateAsync(firstName, lastName);


        // Then
        Assert.Equal(expected, slug);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData(null, null)]
    public async Task InvalidInputs(string? first, string? last)
    {
        var slugGenerator = new EmployeeSlugGenerator();

        await Assert.ThrowsAsync<InvalidOperationException>(async () => await slugGenerator.GenerateAsync(first!, last));
    }


}
