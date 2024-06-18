using System.Text.Json;
using System.Text.Json.Serialization;
using custom_indexing_api.Utils;

public class InputRecords
{
    [JsonRequired]
    [JsonPropertyName("values")]
    public InputRecord[]? Values { get; set; }

    public InputRecords()
    {
        Values = null;
    }
}

public struct InputRecord
{
    [JsonRequired]
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }

    [JsonConverter(typeof(DataContentConverter))]
    [JsonPropertyName("data")]
    public InputDataContent? Data { get; set; }

    //TODO : Implement Errors management
    [JsonPropertyName("errors")]
    public NotificationMessage[] Errors { get; set; }

    [JsonPropertyName("warnings")]
    public NotificationMessage Warnings { get; set; }
}

public class OutputRecords
{
    [JsonRequired]
    [JsonPropertyName("values")]
    public OutputRecord[]? Values { get; set; }

    public OutputRecords()
    {
        Values = null;
    }
}

public struct OutputRecord
{
    [JsonRequired]
    [JsonPropertyName("recordId")]
    public string RecordId { get; set; }

    [JsonConverter(typeof(ChatOutputConverter))]
    [JsonRequired]
    [JsonPropertyName("data")]
    public IChatOutput? Data { get; set; }

    //TODO : Implement Errors management
    [JsonPropertyName("errors")]
    public NotificationMessage[] Errors { get; set; }

    [JsonPropertyName("warnings")]
    public NotificationMessage Warnings { get; set; }
}


public interface InputDataContent
{
    string Text { get; set; }
    string FileName { get; set; }
}

public struct InputFiles : InputDataContent
{
    [JsonRequired]
    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonRequired]
    [JsonPropertyName("filename")]
    public string FileName { get; set; }
}

public class NotificationMessage
{
    [JsonRequired]
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

public interface IChatOutput { }

public class ChatCompletionsOutput : IChatOutput
{
    [JsonRequired]
    [JsonPropertyName("code_description")]
    public string CodeDescription { get; set; }

    [JsonRequired]
    [JsonPropertyName("code_summary")]
    public string CodeSummary { get; set; }
}

public struct ChatDataFlowsOutput : IChatOutput
{
    [JsonRequired]
    [JsonPropertyName("data_flows")]
    public DataFlow[] DataFlows { get; set; }

    [JsonRequired]
    [JsonPropertyName("mermaid")]
    public string Mermaid { get; set; }
}

public struct DataFlow
{
    [JsonRequired]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonRequired]
    [JsonPropertyName("technical_description")]
    public string? TechnicalDescription { get; set; }

    [JsonRequired]
    [JsonPropertyName("functional_description")]
    public string? FunctionalDescription { get; set; }
}