var builder = DistributedApplication.CreateBuilder(args);


var postgresServer = builder.AddPostgres("postgreSQLServer");
var exampleDatabase = postgresServer.AddDatabase("exampleDB");

var apiService = builder.AddProject<Projects.WebShopApi>("apiservice")
    .WaitFor(exampleDatabase);

builder.AddProject<Projects.BlazorApp1>("webfrontend")
    .WaitFor(apiService);

builder.Build().Run();
