using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack.Host;
using ServiceStack;
using System.Collections.Generic;

namespace WildcardRouting
{
    public static class WildcardFunctions
    {
        [FunctionName("WildcardProxy")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, Route = "{*fullPath}")] HttpRequest req, string fullPath,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var requestDto = HostContext.Metadata.CreateRequestFromUrl(fullPath, req.Method);
            var obj = ServiceStack.Text.JsonSerializer.DeserializeFromString(requestBody, requestDto.GetType());
            requestDto.PopulateWithNonDefaultValues(obj);
            var responseType = HostContext.Metadata.GetResponseTypeByRequest(requestDto.GetType());
            var response = AppHost.Instance.GetServiceGateway().Send(responseType, requestDto);
            
            // if the response is a json string we can just return it back up as is
            if(response is string)
            {
                return new ContentResult() { Content = (string)response, ContentType = "application/json" };
            }

            return new OkObjectResult(response); 
        }
    }

   
}
