using MCPLocalFunctionCallPOC.Api.Clients;
using MCPLocalFunctionCallPOC.Api.DTO;
using MCPLocalFunctionCallPOC.Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<GptClient>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/gpt/ask", async (ChatRequest req, GptClient gptClient) =>
{
    var result = await gptClient.Process(req);
    return Results.Ok(result);
});

app.Run();

