using Microsoft.EntityFrameworkCore;
using orders.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Bind HTTPS to port 5080
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5080, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Add controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRPolicy", policy =>
    {
        policy.WithOrigins("null") // allow file:// origins
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register SignalR
builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// CORS MUST be before routing
app.UseCors("SignalRPolicy");

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.UseWebSockets();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Map SignalR hub
app.MapHub<OrderStatusHub>("/order-status");

// Exception handler
app.UseExceptionHandler("/error");

// Dev-only OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
