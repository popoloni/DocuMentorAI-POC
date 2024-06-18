using System.Text.Json;
using System.Text.Json.Serialization;

namespace custom_indexing_api.Utils
{
    public class ChatOutputConverter : JsonConverter<IChatOutput>
    {
        public override IChatOutput Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var jsonObject = jsonDoc.RootElement;

                // Check the content of the JSON object to decide which class to instantiate
                if (jsonObject.TryGetProperty("data_flows", out var _))
                {
                    return JsonSerializer.Deserialize<ChatDataFlowsOutput>(jsonObject.GetRawText(), options);
                }
                else if (jsonObject.TryGetProperty("code_summary", out var _) && jsonObject.TryGetProperty("code_description", out var _))
                {
                    return JsonSerializer.Deserialize<ChatCompletionsOutput>(jsonObject.GetRawText(), options);
                }
                else
                {
                    throw new JsonException($"Invalid JSON object for type : {nameof(IChatOutput)}");
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, IChatOutput value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}