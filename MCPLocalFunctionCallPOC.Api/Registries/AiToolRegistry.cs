using MCPLocalFunctionCallPOC.Api.Tools;

namespace MCPLocalFunctionCallPOC.Api.Registries;

public class AiToolRegistry : List<IAiTool>
{
    public IAiTool? this[string? name] =>
        !string.IsNullOrEmpty(name)
            ? this.FirstOrDefault(t => t.GetDefinition().name == name) ?? throw new KeyNotFoundException()
            : null;
}