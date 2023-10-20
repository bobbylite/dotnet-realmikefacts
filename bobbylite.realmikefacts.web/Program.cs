using bobbylite.realmikefacts.web.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddMvc();
builder.AddAuthentication();
builder.AddAuthorization();

WebApplication app = builder.Build();

app.AddMiddleware();
app.AddEndpoints();

app.Run();
