using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Xsl;
using Azure;
using Azure.AI.OpenAI;
using custom_indexing_api.Utils;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace custom_indexing_api
{
    public class CustomIndexer
    {
        private readonly ILogger _logger;
        private readonly GPTUtils _gptUtils;

        public CustomIndexer(ILoggerFactory loggerFactory, GPTUtils GPTUtils)
        {
            _logger = loggerFactory.CreateLogger<CustomIndexer>();
            _gptUtils = GPTUtils;
        }

        [Function(nameof(EnrichIndex))]
        public async Task<HttpResponseData> EnrichIndex(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [FromBody] InputRecords records)
        {
            // If no body is sent to the API
            if (records is null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                badRequestResponse.WriteString("Please pass a valid request body");
                return badRequestResponse;
            }

            var tasks = new List<Task<Response<ChatCompletions>>>();
            foreach (var record in records.Values)
            {
                //Call GPT for each record
                tasks.Add(_gptUtils.CallGPTAsync(
                    new List<ChatRequestMessage>
                    {
                        // The system message represents instructions or other guidance about how the assistant should behave
                        new ChatRequestSystemMessage(PROMPTS.PROMPT_CODE_DESCRIPTION),
                        // User messages represent current or historical input from the end user
                        new ChatRequestUserMessage(FEWSHOT_CONFIG.CODE_DESC_ASSISTANT_USER),
                        //This message represents the expected output format.
                        new ChatRequestAssistantMessage(FEWSHOT_CONFIG.CODE_DESC_ASSISTANT_PROMPT),
                        //Here is the actual ask from the user
                        new ChatRequestUserMessage($"FILE NAME: {record.Data.FileName} \n###\nFILE CONTENT:\n{record.Data.Text}")
                    }));
            }

            //Send all calls to the AOAI GPT Endpoint
            await Task.WhenAll(tasks.ToArray());

            //Create a list of responses with the recordId, contractText and response from GPT
            var enrichedRecords = new OutputRecords()
            {
                Values = tasks.Select((t, index) =>
                {
                    //Cloning the initial data content
                    var data = records.Values[index].Data;

                    var output = t.Result.Value.Choices[0].Message.Content;

                    _logger.LogInformation($"GPT Response: {output}");

                    //Set with the response from GPT completion API
                    IChatOutput chatOutput =
                        JsonSerializer.Deserialize<ChatCompletionsOutput>(output);

                    return new OutputRecord
                    {
                        //Set input with the recordId and contractText that helped witht the openai request
                        RecordId = records.Values[index].RecordId,
                        //Fill in the data with formatted response from GPT
                        Data = chatOutput,
                        Errors = null,
                        Warnings = null
                    };
                }).ToArray()
            };

            //Display all outputs in the same response;
            var enrichedIndexResult = req.CreateResponse(HttpStatusCode.OK);
            enrichedIndexResult.Headers.Add("Content-Type", "application/json; charset=utf-8");

            enrichedIndexResult.WriteString($"{JsonSerializer.Serialize(enrichedRecords)}");
            return enrichedIndexResult;
        }

        [Function(nameof(ExtractDataFlows))]
        public async Task<HttpResponseData> ExtractDataFlows(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [FromBody] InputRecords records)
        {
            // If no body is sent to the API
            if (records is null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
                badRequestResponse.WriteString("Please pass a valid request body");
                return badRequestResponse;
            }

            //CallGptAsync for each record
            var tasks = new List<Task<Response<ChatCompletions>>>();
            foreach (var record in records.Values)
            {
                //Call GPT for each record
                tasks.Add(_gptUtils.CallGPTAsync(
                    new List<ChatRequestMessage>
                    {
                        // The system message represents instructions or other guidance about how the assistant should behave
                        new ChatRequestSystemMessage(PROMPTS.PROMPT_DATA_FLOWS),
                        //Here is the actual ask from the user
                        new ChatRequestUserMessage($"FILE NAME: {record.Data.FileName} \n###\nFILE CONTENT:\n{record.Data.Text}")
                    }));
            }

            //Send all calls to the AOAI GPT Endpoint
            await Task.WhenAll(tasks.ToArray());

            //Create a list of responses with the recordId, contractText and response from GPT
            var extractedDataFlows = new OutputRecords()
            {
                Values = tasks.Select((t, index) =>
                {
                    //Cloning the initial data content
                    var data = records.Values[index].Data;

                    var output = t.Result.Value.Choices[0].Message.Content;

                    _logger.LogInformation($"GPT Response: {output}");

                    //Set with the response from GPT completion API
                    IChatOutput chatOutput =
                        JsonSerializer.Deserialize<ChatDataFlowsOutput>(output);

                    return new OutputRecord
                    {
                        //Set input with the recordId and contractText that helped witht the openai request
                        RecordId = records.Values[index].RecordId,
                        //Fill in the data with formatted response from GPT
                        Data = chatOutput,
                        Errors = null,
                        Warnings = null
                    };
                }).ToArray()
            };

            //Display all outputs in the same response;
            var extractedDataFlowsResult = req.CreateResponse(HttpStatusCode.OK);
            extractedDataFlowsResult.Headers.Add("Content-Type", "application/json; charset=utf-8");

            extractedDataFlowsResult.WriteString($"{JsonSerializer.Serialize(extractedDataFlows)}");
            return extractedDataFlowsResult;
        }
    }
}
