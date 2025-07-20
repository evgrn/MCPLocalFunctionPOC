namespace MCPLocalFunctionCallPOC.Api.Models;

public class FunctionDefinition
{
    public FunctionDefinition()
    {
    }

    public FunctionDefinition(string name, string description, FunctionParameters parameters)
    {
        this.name = name;
        this.description = description;
        this.parameters = parameters;
    }

    public string name { get; set; }
    public string description { get; set; }
    public FunctionParameters parameters { get; set; }
}

public class FunctionParameters
{
    public FunctionParameters()
    {
    }

    public FunctionParameters(string type, Dictionary<string, FunctionProperty> properties, string[] required)
    {
        this.type = type;
        this.properties = properties;
        this.required = required;
    }

    public string type { get; set; }
    public Dictionary<string, FunctionProperty> properties { get; set; }
    public string[] required { get; set; }
}

public class FunctionProperty
{
    public FunctionProperty()
    {
    }

    public FunctionProperty(string type, string description)
    {
        this.type = type;
        this.description = description;
    }

    public string type { get; set; }
    public string description { get; set; }
}