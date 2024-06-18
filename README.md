# AI code documentation generator

## Disclaimer:
This sample script is not supported under any Microsoft standard support program or service. <br/>
The sample script is provided AS IS without warranty of any kind. Microsoft further disclaims <br/>
all implied warranties including, without limitation, any implied warranties of merchantability <br/>
or of fitness for a particular purpose. The entire risk arising out of the use or performance of <br/>
the sample scripts and documentation remains with you. In no event shall Microsoft, its authors, <br/>
or anyone else involved in the creation, production, or delivery of the scripts be liable for any <br/>
damages whatsoever (including, without limitation, damages for loss of business profits, business <br/>
interruption, loss of business information, or other pecuniary loss) arising out of the use of or <br/>
inability to use the sample scripts or documentation, even if Microsoft has been advised of the <br/>
possibility of such damages <br/>

## Pre-requisites

Before starting to analyse any codebase, you will need to have the following resource deployed on Azure:

- **Azure Storage Account**: to store the code files

- **Azure AI Search** - with GPT4-turbo (128k) deployed: to index and analyse the code files

- **Azure Function App (Python)**: to run the custom skills

## How-to: Configure AI Search index

Before you can use the AI search index, you need to configure it. There's **5 steps** you need to follow:

1 - Create a AI Search instance on Azure (if you don't have one already)

2 - Create a new data source pointing to the Storage Account where your code files are stored. \***_the code files in a blob container will be indexxed as one application_**

3 - Create a new skillset based on the JSON file: [skillset.json](/src/ai-search-config/skillset.json)

4 - Create a new index based on the JSON file: [index.json](/src/ai-search-config/index.json)

5 - Create a new indexer based on the JSON file: [indexer.json](/src/ai-search-config/indexer.json)

## How-to: Analyse the application and Build the AI Search index

After you have configured the AI Search index, you can start the analysis and build the index.

In order to do that, simply launch the indexxer and once it's ready, you can start querying the index to check the results.

## Chat with the index and generate the documentation

Now that the analysis is done, you can start learning about your application by:

- Chatting with the index - the easiest way to start is by using the **_chat on your data_** feature in the [OAI portal](https://oai.azure.com/)
- Generating the documentation - you can use the [python-notebook](/src/python_notebook/notebook.ipynb) to generate the documentation for your application

## Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft
trademarks or logos is subject to and must follow
[Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).
Use of Microsoft trademarks or logos in modified versions of this project must not cause confusion or imply Microsoft sponsorship.
Any use of third-party trademarks or logos are subject to those third-party's policies.
