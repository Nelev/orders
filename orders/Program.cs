using Microsoft.EntityFrameworkCore;
using orders.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Bind HTTPS to port 5080
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5080, listenOptions =>
    {
        listenOptions.UseHttps(); // uses the dev certificate
    });
});

// Add controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// CORS policy (required before UseCors)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register SignalR
builder.Services.AddSignalR();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Add websockets
app.UseWebSockets();


// Add SignalR
app.MapHub<OrderStatusHub>("/order-status");

// Add exception handler
app.UseExceptionHandler("/error");

// Dev-only OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enable CORS
app.UseCors();

// You can keep this — it redirects HTTP → HTTPS,
// but since you're not listening on HTTP, it won't hurt.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
