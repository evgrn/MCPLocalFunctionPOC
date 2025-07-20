using MCPLocalFunctionCallPOC.Api.Models;

namespace MCPLocalFunctionCallPOC.Api.Tools;

public interface IAiTool
{
    public FunctionDefinition GetDefinition();
    public string? Execute(string? param);
    public Type GetResponseType();
}
