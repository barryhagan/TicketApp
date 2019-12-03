using System;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.Common;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TicketApi.GraphQL
{
    internal class TicketGraphQLHttpMiddleware<TSchema> : GraphQLHttpMiddleware<TSchema> where TSchema : ISchema
    {
        private readonly ILogger<TicketGraphQLHttpMiddleware<TSchema>> logger;

        public TicketGraphQLHttpMiddleware(ILogger<TicketGraphQLHttpMiddleware<TSchema>> logger, RequestDelegate next, PathString path, Action<JsonSerializerSettings> configure) : base(next, path, configure)
        {
            this.logger = logger;
        }

        protected override Task RequestExecutedAsync(GraphQLRequest request, int indexInBatch, ExecutionResult result)
        {
            if (result.Errors?.Any() ?? false)
            {
                var thrownExceptions = result.Errors.Select(r => r.InnerException ?? r).ToList();
                Exception logEx = thrownExceptions.Count == 1 ? thrownExceptions.First() : new AggregateException(thrownExceptions);
                logger.LogError(logEx, $"GraphQL API exception during {request.OperationName ?? "request"}: {logEx.Message}.");
            }
            return Task.CompletedTask;
        }
    }
}
