
using Alba;
using ReferenceApi.Order;

namespace ReferenceApi.ContractTests.Orders;
public class PlacingOrders : IClassFixture<HostFixtureNoDatabase>
{

    private readonly IAlbaHost Host;
    public PlacingOrders(HostFixtureNoDatabase fixture)
    {
        Host = fixture.Host;
    }

    [Fact]

    public async Task CanPlaceAnOrder()
    {
        var request = new CreateOrderRequest
        {
            Items = [
                new OrderItemModel {
                    Price = 10.12M,
                    Qty = 12,
                    Sku = "beer"
                }
                ]
        };
        var stubbedBonus = 420.69M;
        var subtotal = 12 * 10.12M;
        var total = subtotal - stubbedBonus;
        var expected = new CreateOrderResponse
        {
            Id = Guid.NewGuid(),
            Discount = stubbedBonus,
            SubTotal = subtotal,
            Total = total
        };

        var response = await Host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/orders");
            api.StatusCodeShouldBeOk();
        });

        var actualBody = await response.ReadAsJsonAsync<CreateOrderResponse>();
        Assert.NotNull(actualBody);
        //Assert.Equal(expected, actualBody);

        Assert.Equal(expected.Total, actualBody.Total);
        Assert.Equal(expected.SubTotal, actualBody.SubTotal);
        Assert.Equal(expected.Discount, actualBody.Discount);
        // not testing the id.??

    }
}
