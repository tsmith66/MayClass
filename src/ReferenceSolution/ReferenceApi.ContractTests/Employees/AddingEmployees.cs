
using Alba;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.Employees;
public class AddingEmployees : IClassFixture<HostFixture>
{

    private readonly IAlbaHost Host;
    public AddingEmployees(HostFixture fixture)
    {
        Host = fixture.Host;
    }

    [Theory]
    [ClassData(typeof(EmployeesSampleData))]
    public async Task CanHireNewEmployees(EmployeeCreateRequest request, string expectedId)
    {
        // Given
        // A Host Per Test (Host Per Class, Collections)

        var expected = new EmployeeResponseItem
        {
            Id = expectedId,
            FirstName = request.FirstName,
            LastName = request.LastName!
        };


        var response = await Host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        var responseMessage = await response.ReadAsJsonAsync<EmployeeResponseItem>();
        Assert.NotNull(responseMessage);

        Assert.Equal(expected, responseMessage);

        var response2 = await Host.Scenario(api =>
        {
            api.Get.Url($"/employees/{responseMessage.Id}");
            api.StatusCodeShouldBeOk();
        });

        var responseMessage2 = await response2.ReadAsJsonAsync<EmployeeResponseItem>();
        Assert.NotNull(responseMessage2);

        Assert.Equal(responseMessage, responseMessage2);
    }

    [Fact]
    public async Task ValidationsAreChecked()
    {
        var request = new EmployeeCreateRequest { FirstName = "", LastName = "" }; // BAD Employee


        var response = await Host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(400);
        });
    }
}


public class AnotherSetOfTests : IClassFixture<HostFixture>
{
    [Fact]
    public async Task DoSomething()
    {
        // new instance here.
    }
}