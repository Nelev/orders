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

// Add routing, session, and authorization middlewares
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Add websockets
app.UseWebSockets();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{ 
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 
    db.Database.Migrate(); 
}

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

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
