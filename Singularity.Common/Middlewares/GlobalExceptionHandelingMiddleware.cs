using Microsoft.AspNetCore.Http;
using Singularity.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using KeyNotFoundException = Singularity.Common.Exceptions.KeyNotFoundException;
using NotImplementedException = Singularity.Common.Exceptions.NotImplementedException;

namespace Singularity.Common.Middlewares
{
    public class GlobalExceptionHandelingMiddleware
    {

        private readonly RequestDelegate _next;
        public GlobalExceptionHandelingMiddleware(RequestDelegate next)
        {
            _next = next;   
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync( context,  ex);
            
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
           HttpStatusCode statusCode;
            var stackTrace = string.Empty;
            string message = "";
            var exceptionType = ex.GetType();

            if (exceptionType ==typeof(NotFoundException))
            {
                message = ex.Message;   
                statusCode = HttpStatusCode.NotFound;   
                stackTrace= ex.StackTrace;  
            }
            else if (exceptionType == typeof(BadRequestException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace;
            }
           else if (exceptionType == typeof(KeyNotFoundException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.NotFound;
                stackTrace = ex.StackTrace;
            }
           else if (exceptionType == typeof(NotImplementedException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.NotImplemented;
                stackTrace = ex.StackTrace;
            }
           else if (exceptionType == typeof(UnAuthorizedAccessException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.Unauthorized;
                stackTrace = ex.StackTrace;
            }
            else 
            {
                message = ex.Message;
                statusCode = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
            }
            var exceptionresult = JsonSerializer.Serialize(new { error = message, stackTrace });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(exceptionresult);

        }
    }
}
