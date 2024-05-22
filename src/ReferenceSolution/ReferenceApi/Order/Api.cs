using Microsoft.AspNetCore.Http.HttpResults;

namespace ReferenceApi.Order;

public static class Api
{
    public static IEndpointRouteBuilder MapOrdersApi(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("orders");
        group.MapGet("/", GetOrdersAsync);
        group.MapPost("/", AddOrderAsync);
        return app;
    }

    public static async Task<Ok> GetOrdersAsync(CancellationToken token)
    {
        return TypedResults.Ok();
    }

    public static async Task<Ok<CreateOrderResponse>> AddOrderAsync(
        CreateOrderRequest request,
        IGetBonusesForOrders client,
        CancellationToken token)
    {
        // OBVIOUSLY NEVER TRUST ANYTHING FROM THE CLIENT - Look up these items and verify the price, etc.
        var subTotal = request.Items.Select(i => i.Qty * i.Price).Sum();

        decimal discount = await client.GetBonusForPurchaseAsync(Guid.NewGuid(), subTotal, token);

        var response = new CreateOrderResponse
        {
            Id = Guid.NewGuid(),
            Discount = discount,
            SubTotal = subTotal,
            Total = subTotal - discount,
        };
        return TypedResults.Ok(response);
    }
}



public record CreateOrderRequest
{
    public IList<OrderItemModel> Items { get; set; } = [];
}

public record OrderItemModel
{
    public string Sku { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal Price { get; set; }
}

public record CreateOrderResponse
{
    public Guid Id { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

}
/*
 * POST /orders
 * 
 * {
 *  items: [
 *      {    sku: "19", qty: 1, price: 2.99 }
 *  ]
 * }
 * 
 * 
 * 201 Created
 * 
 * {
 *  "orderId": "guid",
 *  "subTotal": 2.99,
 *  "discount": 1.12,
 *  "total": 2.99 - 1.12
 * }
 */