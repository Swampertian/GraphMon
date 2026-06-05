
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddGraphQLServer();

var app = builder.Build();

app.MapGraphQL("/pokedex");
app.MapDefaultEndpoints();

app.Run();