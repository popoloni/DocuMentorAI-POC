using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace custom_indexing_api.Utils
{
    public class DataContentConverter : JsonConverter<InputDataContent>
    {
        public override InputDataContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;

            // Check the properties of the JSON object to decide which class to instantiate
            if (jsonObject.TryGetProperty("filename", out var _) && jsonObject.TryGetProperty("text", out var _))
            {
                return JsonSerializer.Deserialize<InputFiles>(jsonObject.GetRawText(), options);
            }
            else
            {
                throw new JsonException($"Invalid JSON object for type : {nameof(InputDataContent)}");
            }
        }

        public override void Write(Utf8JsonWriter writer, InputDataContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}