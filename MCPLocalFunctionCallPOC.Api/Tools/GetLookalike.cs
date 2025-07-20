using MCPLocalFunctionCallPOC.Api.Models;
using Newtonsoft.Json;

namespace MCPLocalFunctionCallPOC.Api.Tools;

public class GetLookalike : IAiTool
{
    public FunctionDefinition GetDefinition() => new(
        nameof(GetLookalike),
        "Retourne le nom de la personne à laquelle celle dont le nom est passé ressemble le plus",
        new FunctionParameters(
            "object",
            new()
            {
                {
                    "PersonName",
                    new FunctionProperty("string",
                        "nom de la personne pour laquelle on veut rechercher le nom de la personnne la plus ressemblante")
                }
            },
            ["PersonName"]));
    
    private string SessionId { get; set; } = string.Empty;
    

    private static List<string> _namesThatLooksLikePedroPascal = new()
    {
        "Manu",
        "Emmanuel",
        "Manu Davant",
        "Emmanuel Davant"
    };


    public FunctionDefinition Definition { get; set; }

    public string? Execute(string? serializedParam)
    {
        var param = JsonConvert.DeserializeObject<GetLookalikeParameter>(serializedParam);
        var resp = new GetLookalikeResponse(
            _namesThatLooksLikePedroPascal
                .Contains(param.PersonName, StringComparer.InvariantCultureIgnoreCase)
                ? "Pedro Pascal"
                : "François Bayrou");
        return JsonConvert.SerializeObject(resp);
    }
    
    public Type GetResponseType() => typeof(GetLookalikeResponse);


    public record GetLookalikeParameter(string PersonName);
    public record GetLookalikeResponse(string LookalikeName);
    
}