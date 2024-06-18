{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "OPENAI_BASEURL": "https://***.openai.azure.com/",
    "OPENAI_KEY": "***",
    "OPENAI_DEPLOYMENTNAME": "gpt-4-32k",
    "OPENAI_SYSTEMPROMPT": "You are an Delphi Expert that helps extract detailed functional description from given source code file.\nTaking the code in input, you should try to extract:\n\n- the high level description of what the code does\n- the list of fields with a description\n- the detailed business rules, function by functio\n- the custom code dependencies: ignore the standard library references\n\n You generate a response with a block for each extracted part, with a title to clearly identifies it, IN MARKDOWN FORMAT.\n It should be easy to understand for a non technical persona.\nYou MUST generate and response in French language.",
    "OPENAI_USERPROMPT": ""
  }
}
