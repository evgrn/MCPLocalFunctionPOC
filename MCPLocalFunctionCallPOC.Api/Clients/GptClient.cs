using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using MCPLocalFunctionCallPOC.Api.DTO;
using MCPLocalFunctionCallPOC.Api.Models;
using MCPLocalFunctionCallPOC.Api.Registries;
using MCPLocalFunctionCallPOC.Api.Tools;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MCPLocalFunctionCallPOC.Api.Clients;

public class GptClient(IOptions<AppSettings> appSettings)
{
    private readonly string _openApiKey = appSettings.Value.OpenAiKey; 
    private const string Endpoint = "https://api.openai.com/v1/chat/completions";
    private static readonly HttpClient _client = new();

    public  async Task<string> Process(ChatRequest req)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openApiKey);

        var messages = new[]
        {
            new { role = "system", content = "Tu peux appeler des fonctions pour répondre." },
            new { role = "user", content = req.Prompt }
        };


        var tools = new AiToolRegistry() { new GetLookalike() };
        
        var requestBody = new
        {
            model = "gpt-4o",
            messages,
            functions = tools.Select(t => t.GetDefinition()).ToList(),
            function_call = "auto"
        };

        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(Endpoint, content);
        var respString = await response.Content.ReadAsStringAsync();
        
        var result = JObject.Parse(respString);

        var message = result["choices"]?[0]?["message"];
        var funcCall = message?["function_call"];

        if (funcCall != null)
        {
            var funcName = funcCall["name"]?.ToString();
            var func = tools[funcName];
            var stringResp = func?.Execute(funcCall["arguments"]?.ToString());
            var resp = JsonConvert.DeserializeObject(stringResp, func.GetType());
            var followUp = new
            {
                model = "gpt-4o",
                messages = new object[]
                {
                    messages[0],
                    messages[1],
                    message,
                    new { role = "function", name = funcName, content = stringResp }
                }
            };

            var followContent = new StringContent(JsonConvert.SerializeObject(followUp), Encoding.UTF8, "application/json");
            var followUpResponse = await _client.PostAsync(Endpoint, followContent);
            var followUpResult = JObject.Parse(await followUpResponse.Content.ReadAsStringAsync());

            var finalReply = followUpResult["choices"]?[0]?["message"]?["content"]?.ToString();
            
            return $"{(finalReply ?? "Aucune réponse générée.")}";
        }

        return message?["content"]?.ToString() ?? "Pas de réponse GPT.";
    }
    
}