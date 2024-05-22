

using NSubstitute;
using ReferenceApi.Employees;

namespace ReferenceApi.UnitTests;
public class SlugGeneratorWithUniqueIdsTests
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
        var fakeUniqueChecker = Substitute.For<ICheckForUniqueEmployeeStubs>();
        fakeUniqueChecker.CheckUniqueAsync(expected, CancellationToken.None).Returns(true);

        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(fakeUniqueChecker);


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
        var fakeUniqueChecker = Substitute.For<ICheckForUniqueEmployeeStubs>();
        fakeUniqueChecker.CheckUniqueAsync(Arg.Any<string>(), CancellationToken.None).Returns(true);

        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(fakeUniqueChecker);


        await Assert.ThrowsAsync<InvalidOperationException>(async () => await slugGenerator.GenerateAsync(first!, last));
    }
    [Theory]
    [InlineData("Johnny", "Marr", "marr-johnny-a")]
    [InlineData("Jeff", "Gonzalez", "gonzalez-jeff-a")]
    [InlineData("Jeff", "Gonzalez", "gonzalez-jeff-z")]
    public async Task DuplicatesCreateUniqueSlugs(string firstName, string lastName, string expected)
    {
        var fakeUniqueChecker = Substitute.For<ICheckForUniqueEmployeeStubs>();
        fakeUniqueChecker.CheckUniqueAsync(expected, CancellationToken.None).Returns(true);
        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(fakeUniqueChecker);

        var slug = await slugGenerator.GenerateAsync(firstName, lastName);

        Assert.Equal(expected, slug);
    }



    [Fact]
    public async Task AUniqueIdIsAdded()
    {
        var fakeUniqueChecker = Substitute.For<ICheckForUniqueEmployeeStubs>();
        //fakeUniqueChecker.CheckUniqueAsync(Arg.Any<string>(), CancellationToken.None).Returns(false);
        var slugGenerator = new EmployeeSlugGeneratorWithUniqueIds(fakeUniqueChecker);
        var slug = await slugGenerator.GenerateAsync("Dog", "Man");

        Assert.StartsWith("man-dog", slug);
        Assert.True(slug.Length == 7 + 22); // Is the slug with 22 random thingies on the end.

    }

}

//public class AlwaysUniqueDummy : ICheckForUniqueEmployeeStubs
//{
//    public Task<bool> CheckUniqueAsync(string slug, CancellationToken token)
//    {
//        return Task.FromResult(true);
//    }
//}


//public class NeverUniqueDummy : ICheckForUniqueEmployeeStubs
//{
//    public Task<bool> CheckUniqueAsync(string slug, CancellationToken token)
//    {
//        return Task.FromResult(false);
//    }
//}

