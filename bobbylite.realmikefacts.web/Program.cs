using bobbylite.realmikefacts.web.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddAuthentication();
builder.AddAuthorization();
builder.AddHttpClients();
builder.AddServices();
builder.AddCors();

WebApplication app = builder.Build();

app.AddMiddleware();
app.AddEndpoints();

app.Run();
