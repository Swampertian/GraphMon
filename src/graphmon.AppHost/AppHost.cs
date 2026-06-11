var builder = DistributedApplication.CreateBuilder(args);

var postgresUsername = builder.AddParameter("postgres-user");
var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var postgres = builder
    .AddPostgres("postgres", userName: postgresUsername, password: postgresPassword)
    .WithImageTag("latest")
    .WithVolume("postgres-data", "/var/lib/postgresql/")
    .WithHostPort(36135)
    .WithPgAdmin();

var pokedexDb = postgres.AddDatabase("pokedex");

builder
    .AddProject<Projects.Pokedex_Api>("pokedex-api")
    .WithReference(pokedexDb)
    .WaitFor(postgres);

builder.Build().Run();
