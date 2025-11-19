using Microsoft.AspNetCore.Http;
using MISA.QLTS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidateException ex)
            {
                await HandleExceptionAsync(context, ex.Message, 200);
            }
            catch (BusinessException ex)
            {
                await HandleExceptionAsync(context, ex.Message, 200);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex.Message, 404);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex.Message, 500);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, string message, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = ApiResponse<object>.Fail(message);

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
