

using Alba;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ReferenceApi.Order;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ReferenceApi.ContractTests.Orders;
public class PlacingOrdersWm
{
    [Fact]
    public async Task StubbingHttpCalls()
    {
        var server = WireMockServer.Start();

        //        const string expectedRequestBody = %
        //"""
        //                    {
        //                        "orderTotal": 121.44,
        //                        "purchaseDate": "{DateTimeOffset.Now}"
        //                    }
        //                    """;
        var clockTimeForTest = new DateTimeOffset(new DateTime(1969, 4, 20), TimeSpan.FromHours(-4));

        var fakeTime = new FakeTimeProvider(clockTimeForTest);
        var expectedBody = new CustomerLoyaltyTypes.LoyaltyDiscountRequest
        {
            OrderTotal = 121.44,
            //PurchaseDate = fakeTime.GetLocalNow(),
        };
        server.Given(
            Request.Create().WithPath("/customers/*/purchase-rewards")
            .UsingPost()
            .WithBodyAsJson(expectedBody)
            ).RespondWith(
                Response
                .Create()
                .WithBodyAsJson(new CustomerLoyaltyTypes.LoyaltyDiscountResponse
                {
                    DiscountAmount = 420.69
                }
                )
            );
        var request = new CreateOrderTestingRequest
        {
            Items = [
               new OrderItemTestingModel {
                    Price = 10.12M,
                    Qty = 12,
                    Sku = "beer"
                }
               ]
        };
        // calling MY endpoing for orders with post


        var host = await AlbaHost.For<Program>(config =>
        {
            config.UseSetting("loyaltyApi", server.Url);
            config.ConfigureTestServices(sp =>
            {
                sp.AddSingleton<TimeProvider>(fakeTime);
            });
        });

        var response = await host.Scenario(api =>
        {
            api.Post.Json(request).ToUrl("/orders");
            api.StatusCodeShouldBeOk();
        });


        var actualBody = await response.ReadAsJsonAsync<CreateOrderResponse>();
        Assert.NotNull(actualBody);
        //Assert.Equal(expected, actualBody);
        Assert.Equal(420.69M, actualBody.Discount);
        var entries = server.LogEntries;
        //server.Should().HaveReceivedACall().AtUrl()
        var x = 12;
    }
}

// My copies of the types
public record CreateOrderTestingRequest
{
    public IList<OrderItemTestingModel> Items { get; set; } = [];
}

public record OrderItemTestingModel
{
    public string Sku { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}