using Maibro.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Primitives;
using Oversite.DTO.Request;
using Oversite.PublicApi.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Oversite.PublicApi.Middleware
{

    public class DecryptQueryMiddleware
    {
        private readonly RequestDelegate _next;

        public DecryptQueryMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        // Type parsing logic based on the expected type
        private object ParseType(string value, Type targetType)
        {
            if (targetType == typeof(int) && int.TryParse(value, out var intValue))
                return intValue;
            if (targetType == typeof(bool) && bool.TryParse(value, out var boolValue))
                return boolValue;
            if (targetType == typeof(float) && float.TryParse(value, out var floatValue))
                return floatValue;
            if (targetType == typeof(DateTime) && DateTime.TryParse(value, out var dateValue))
                return dateValue;

            return value;  // Return the value as string for unknown types or strings
        }

        // Retrieves the expected type for the query parameter by reflecting on the DTO used by the API action
        private Type GetExpectedTypeForParam(HttpContext context, string paramName)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null) return typeof(string);  // Default to string if endpoint is not found

            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (actionDescriptor == null) return typeof(string);  // Default to string if action descriptor is not found

            var parameters = actionDescriptor.MethodInfo.GetParameters();
            foreach (var parameter in parameters)
            {
                // Check if the parameter is a complex type (e.g., a DTO)
                if (parameter.ParameterType.IsClass && parameter.ParameterType != typeof(string))
                {
                    var property = parameter.ParameterType.GetProperty(paramName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        return property.PropertyType;  // Return the property type
                    }
                }
            }

            return typeof(string);  // Default fallback
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var aesService = new AesEncryptionService();

            // Only apply this logic to GET requests
            if (context.Request.Method != HttpMethods.Get)
            {
                await _next(context);  // Skip for non-GET requests
                return;
            }

            var decryptedParams = new Dictionary<string, string>();

            foreach (var queryParam in context.Request.Query)
            {
                var requestContent1 = (new EncryptDecryptUtil().DecodeFrom64(queryParam.Value.ToString()));

                string ob = new EncryptDecryptUtil().DecryptProperty(requestContent1);
                string json = Regex.Replace(ob.Replace(@"\", ""), "^\"|\"$", "");

                var decryptedValue = json;
                //var decryptedValue = aesService.Decode(queryParam.Value);
                decryptedParams[queryParam.Key] = decryptedValue;
            }

            var parsedParams = new Dictionary<string, StringValues>();
            foreach (var param in decryptedParams)
            {
                // Fetch the expected type based on the API action's DTO
                var expectedType = GetExpectedTypeForParam(context, param.Key);

                // Parse the decrypted value to the correct type
                var parsedValue = ParseType(param.Value, expectedType);

                // Format strings with quotes to preserve their original format
                if (parsedValue is string stringValue)
                {
                    //parsedParams[param.Key] = $"\"{stringValue}\"";
                    parsedParams[param.Key] = stringValue.Replace("\\", "").Replace("\"", "");
                }
                else
                {
                    parsedParams[param.Key] = parsedValue.ToString();  // Convert to string for numeric and date types
                }
            }

            // Override the query parameters with parsed values
            context.Request.Query = new QueryCollection(parsedParams);

            await _next(context);
        }
    }

}
