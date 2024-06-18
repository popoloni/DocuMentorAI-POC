using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;

namespace custom_indexing_api.Utils
{
    public class GPTUtils
    {
        private readonly Uri azureOpenAIResourceUri;
        private readonly AzureKeyCredential azureOpenAIApiKey;
        private readonly OpenAIClient client;
        private readonly string deploymentName;


        // Default constructor
        public GPTUtils(IConfiguration configuration)
        {
            azureOpenAIResourceUri = new(configuration.GetValue<string>("OPENAI_BASEURL"));
            azureOpenAIApiKey = new(configuration.GetValue<string>("OPENAI_KEY"));
            deploymentName = configuration.GetValue<string>("OPENAI_DEPLOYMENTNAME");
            client = new(azureOpenAIResourceUri, azureOpenAIApiKey);
        }

        public async Task<Response<ChatCompletions>> CallGPT(IList<ChatRequestMessage> messages)
        {
            var chatCompletionsOptions = new ChatCompletionsOptions()
            {
                DeploymentName = this.deploymentName, // Use DeploymentName for "model" with non-Azure clients
                Temperature = (float)0.7,
                ChoiceCount = 1,

                // StopSequences = { "}" },
                ResponseFormat = ChatCompletionsResponseFormat.JsonObject
            };

            //Adding all the messages to the prompt
            foreach (var message in messages)
            {
                chatCompletionsOptions.Messages.Add(message);
            }

            return await client.GetChatCompletionsAsync(chatCompletionsOptions);
        }

        public async Task<Response<ChatCompletions>> CallGPTAsync(IList<ChatRequestMessage> messages)
        {
            // Define a retry policy
            AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>() // Specify the type of exceptions to handle
                .WaitAndRetryAsync(3, // Retry 3 times
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Use exponential backoff
                    (exception, timeSpan, retryCount, context) =>
                    {
                        // This is a delegate that will be run before each retry. Use it to log retries or perform other actions.
                        Console.WriteLine($"Retry {retryCount} scheduled to happen after {timeSpan.Seconds} seconds due to {exception.Message}.");
                    });

            // Use the retry policy
            try
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {        // This is the code that will be retried if it throws an exception
                    return await CallGPT(messages);
                });
            }
            catch (Exception ex)
            {
                // This code will be run if all retries fail
                Console.WriteLine($"All retries failed: {ex.Message}");
                return null;
            }
        }
    }
}