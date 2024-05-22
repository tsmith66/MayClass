using FluentValidation;
using Marten;
using Microsoft.FeatureManagement;
using ReferenceApi.Employees;
using ReferenceApi.Order;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("data") ?? throw new Exception("No Connection String");

builder.Services.AddMarten(config =>
{
    config.Connection(connectionString);
}).UseLightweightSessions();

var loyaltyApiUrl = builder.Configuration.GetValue<string>("loyaltyApi") ?? throw new Exception("Configuration does not have a url for customer loyalty");
builder.Services.AddHttpClient<CustomerLoyaltyHttpClient>(client =>
{
    client.BaseAddress = new Uri(loyaltyApiUrl);
});

builder.Services.AddScoped<IGetBonusesForOrders>(sp =>
    sp.GetRequiredService<CustomerLoyaltyHttpClient>()
);

builder.Services.AddSingleton(sp => TimeProvider.System);
builder.Services.AddSingleton<INotifyOfPossibleSithLords, NotifyOfPossibleSithLords>();
builder.Services.AddScoped<ICheckForUniqueEmployeeStubs, EmployeeUniquenessChecker>();
builder.Services.AddScoped<IGenerateSlugsForNewEmployees, EmployeeSlugGeneratorWithUniqueIds>();
// Add services to the container.
builder.Services.AddFeatureManagement(); // In Configration, look for "FeatureManagement"
builder.Services.AddValidatorsFromAssemblyContaining<EmployeeCreateRequestValidator>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
//if (await app.Services.GetRequiredService<IFeatureManager>().IsEnabledAsync("Orders"))
//{
//    app.MapOrdersApi();
//}

if (app.Environment.IsDevelopment())
{
    app.MapOrdersApi();
}
app.Run();


public partial class Program { }