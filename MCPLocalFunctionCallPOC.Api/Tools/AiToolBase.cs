using System.Runtime.CompilerServices;
using MCPLocalFunctionCallPOC.Api.Models;

namespace MCPLocalFunctionCallPOC.Api.Tools;

public abstract class AiToolBase<T,TY>
{
     public FunctionDefinition Definition { get; }
     public abstract TY Execute(T param);
     
     public abstract T DeserializeParameter(string serializedParam);
}