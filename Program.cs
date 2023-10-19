using general.purpose.poc.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddMvc();
builder.AddAuthentication();
builder.AddAuthorization();

WebApplication app = builder.Build();

app.AddMiddleware();
app.AddEndpoints();

app.Run();
