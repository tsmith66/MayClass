using Alba;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReferenceApi.Employees;

namespace ReferenceApi.ContractTests.Employees;
public class AddingSithGivesNotification
{

    [Fact]
    public async Task Notifies()
    {
        var mockedSithNotifier = Substitute.For<INotifyOfPossibleSithLords>();
        var host = await AlbaHost.For<Program>(config =>
        {
            config.ConfigureTestServices(services =>
            {
                services.AddScoped<INotifyOfPossibleSithLords>(sp => mockedSithNotifier);
            });
        });
        var request = new EmployeeCreateRequest { FirstName = "Darth", LastName = "Vader" };
        await host.Scenario((api) =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        //THEN
        // Assert on what??
        mockedSithNotifier
            .Received(1)
            .Notify("Darth", "Vader");
    }
    [Fact]
    public async Task DoesNotNotifyForNonSith()
    {
        var mockedSithNotifier = Substitute.For<INotifyOfPossibleSithLords>();
        var host = await AlbaHost.For<Program>(config =>
        {
            config.ConfigureTestServices(services =>
            {
                services.AddScoped<INotifyOfPossibleSithLords>(sp => mockedSithNotifier);
            });
        });
        var request = new EmployeeCreateRequest { FirstName = "Anakin", LastName = "Skywalker" };
        await host.Scenario((api) =>
        {
            api.Post.Json(request).ToUrl("/employees");
            api.StatusCodeShouldBe(201);
        });

        //THEN
        // Assert on what??
        mockedSithNotifier
            .DidNotReceive()
            .Notify(Arg.Any<string>(), Arg.Any<string>());
    }
}
