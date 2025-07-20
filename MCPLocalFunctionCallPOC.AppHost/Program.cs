var builder = DistributedApplication.CreateBuilder(args);

builder
    .AddProject<Projects.MCPLocalFunctionCallPOC_Api>("api")
    .WithEndpoint( 7001);

builder.Build().Run();