namespace ReferenceApi.Order;

public class CustomerLoyaltyHttpClient(HttpClient client, TimeProvider timeProvider) : IGetBonusesForOrders
{

    public async Task<decimal> GetBonusForPurchaseAsync(
        Guid customerId,
        decimal orderTotal,
        CancellationToken token = default)
    {
        var resource = $"/customers/{customerId}/purchase-rewards";
        var request = new CustomerLoyaltyTypes.LoyaltyDiscountRequest()
        {
            OrderTotal = (double)orderTotal,
            PurchaseDate = timeProvider.GetLocalNow(),

        };

        var response = await client.PostAsJsonAsync(resource, request, cancellationToken: token);

        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadFromJsonAsync<CustomerLoyaltyTypes.LoyaltyDiscountResponse>();
        if (body is not null)
        {
            return (decimal)body.DiscountAmount;
        }
        else
        {
            // throw an exception? not sure... (more in a minute)
            return 0;
        }
    }
}


public interface ISystemTime
{
    public DateTimeOffset GetNow();
}

public class SystemTime : ISystemTime
{
    public DateTimeOffset GetNow()
    {
        return DateTimeOffset.Now;
    }
}