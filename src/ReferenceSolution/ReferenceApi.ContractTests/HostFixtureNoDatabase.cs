using Alba;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using ReferenceApi.Order;

namespace ReferenceApi.ContractTests;
public class HostFixtureNoDatabase : IAsyncLifetime
{
    public IAlbaHost Host = null!;



    public async Task InitializeAsync()
    {


        Host = await AlbaHost.For<Program>(config =>
        {
            var fakeLoyaltyApi = Substitute.For<IGetBonusesForOrders>();
            fakeLoyaltyApi.GetBonusForPurchaseAsync(Arg.Any<Guid>(), Arg.Any<decimal>(), Arg.Any<CancellationToken>()).Returns(420.69M);

            config.ConfigureTestServices(sp =>
            {
                sp.AddScoped<IGetBonusesForOrders>((_ => fakeLoyaltyApi));
            });
        });
    }
    public async Task DisposeAsync()
    {

        await Host.DisposeAsync();
    }

}
