using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Business.CodeExecution.Private;
using Business.CodeExecution.ViewModels;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Shared.Framework.Dependency;

namespace Business.CodeExecution
{
    public class CodeExecutor : ICodeExecutor, IDependency
    {
        private const string IdeOneBaseUrl = "http://ideone.com/";
        private const string IdeOneSubmissionPostfix = "ideone/Index/submit/";
        private const string CodeExecutionResultPostfixTemplate = "ideone/Index/view/id/{0}/ajax/1/";
        private const int InlineFormFieldsNumber = 3;
        private const int CodeExecutionResultCheckTimeout = 500;

        public CodeExecutionResponse Execute(CodeExecutionRequest codeExecutionRequest)
        {
            var executionRequestData = ComposeExecutionRequestData();

            var executionResultLinkId = GetExecutionResultLinkId(codeExecutionRequest, executionRequestData);

            return GetCodeExecutionResult(executionResultLinkId);
        }

        private CodeExecutionResponse GetCodeExecutionResult(string executionResultLinkId)
        {
            var requestUrl = string.Format(IdeOneBaseUrl + CodeExecutionResultPostfixTemplate, executionResultLinkId);

            while (true)
            {
                string httpResponse;

                using (var client = new CookieAwareWebClient())
                {
                    httpResponse = client.DownloadString(requestUrl);
                }

                var responseObject = (dynamic)JsonConvert.DeserializeObject(httpResponse);

                if ((CodeExecutionStatus)responseObject.status != CodeExecutionStatus.Running
                    && (CodeExecutionStatus)responseObject.status != CodeExecutionStatus.Compilation
                    && (CodeExecutionStatus)responseObject.status != CodeExecutionStatus.WaitingForCompilation)
                {
                    return ConvertHttpResponseToExecutionResponse(responseObject);
                }

                Task.Delay(CodeExecutionResultCheckTimeout);
            }
        }

        private CodeExecutionResponse ConvertHttpResponseToExecutionResponse(dynamic httpResponse)
        {
            var executionResult = new CodeExecutionResponse();

            var errorStringBuilder = new StringBuilder();
            var stderr = (string) httpResponse.stderr;
            if (!string.IsNullOrEmpty(stderr))
            {
                errorStringBuilder.AppendLine(stderr);
            }
            var cmperr = (string)httpResponse.cmperr;
            if (!string.IsNullOrEmpty(cmperr))
            {
                errorStringBuilder.AppendLine(cmperr);
            }
            if (errorStringBuilder.Length > 0)
            {
                executionResult.ErrorMessage = errorStringBuilder.ToString();
            }
            else
            {
                executionResult.Output = ((string)httpResponse.stdout).Split(Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries);
            }

            return executionResult;
        }

        private string GetExecutionResultLinkId(CodeExecutionRequest codeExecutionRequest,
            CodeExecutionRequestData requestData)
        {
            using (var client = new CookieAwareWebClient())
            {
                client.CookieContainer.Add(requestData.CookieCollection);
                
                client.UploadValues(IdeOneBaseUrl + IdeOneSubmissionPostfix,
                    GetCodeSubmissionRequestParameters(codeExecutionRequest, requestData));

                return client.ResponseUri.AbsolutePath.Replace("/", "");
            }
        }

        private NameValueCollection GetCodeSubmissionRequestParameters(CodeExecutionRequest codeExecutionRequest, 
            CodeExecutionRequestData requestData)
        {
            var nameValueCollection = new NameValueCollection();

            foreach (var protectionFieldValue in requestData.ProtectionFieldValues)
            {
                nameValueCollection.Add(protectionFieldValue.Key, protectionFieldValue.Value);
            }

            nameValueCollection.Add("file", codeExecutionRequest.SourceCode);
            nameValueCollection.Add("input", PrepareInputParameters(codeExecutionRequest.Input));
            nameValueCollection.Add("timelimit", "0");
            nameValueCollection.Add("_lang", ((int)codeExecutionRequest.CodeLanguage).ToString());
            nameValueCollection.Add("public", "0");
            nameValueCollection.Add("run", "1");

            return nameValueCollection;
        }

        private string PrepareInputParameters(IEnumerable<string> input)
        {
            var inputBuilder = new StringBuilder();

            if (input != null)
            {
                foreach (var inputParameter in input)
                {
                    inputBuilder.AppendLine(inputParameter);
                }
            }

            return inputBuilder.ToString();
        }

        private CodeExecutionRequestData ComposeExecutionRequestData()
        {
            var requestData = new CodeExecutionRequestData();

            string httpResponse;

            using (var client = new CookieAwareWebClient())
            {
                httpResponse = client.DownloadString(IdeOneBaseUrl);
                requestData.CookieCollection = client.ResponseCookies;
            }

            requestData.ProtectionFieldValues = GetProtectedFieldValues(httpResponse);

            return requestData;
        }

        private Dictionary<string, string> GetProtectedFieldValues(string httpResponse)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(httpResponse);

            var parametersDictionary = new Dictionary<string, string>();
            var formFieldNumber = 1;

            while (true)
            {
                var parameterName = "p" + formFieldNumber;

                var parameterValue = document.GetElementbyId(parameterName).Attributes["value"].Value;

                parametersDictionary.Add(parameterName, parameterValue);

                formFieldNumber++;

                if (formFieldNumber > InlineFormFieldsNumber)
                {
                    break;
                }
            }

            CalculateProtectedFieldValue(parametersDictionary);
            return parametersDictionary;
        }

        private void CalculateProtectedFieldValue(Dictionary<string, string> parametersDictionary)
        {
            var p4 = 0;

            var p2 = int.Parse(parametersDictionary["p2"]);
            var p3 = int.Parse(parametersDictionary["p3"]);

            for (int i = 0; i < p3; i++)
            {
                p4 += p2 * i;
            }

            parametersDictionary.Add("p4", p4.ToString());
        }
    }
}
