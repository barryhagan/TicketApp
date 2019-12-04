using GraphQL.Types;
using System;
using System.Linq;
using TicketBusinessLogic;
using TicketCore.Dto;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    internal class SearchResultSetGraphType : ObjectGraphType<SearchResultSet>
    {
        public SearchResultSetGraphType(BusinessLogic logic)
        {
            Name = "SearchResultSet";

            Field<ListGraphType<ScoredSearchResultGraphType<Organization>>>()
                .Name("organizations")
                .ResolveAsync(async ctx =>
                {
                    var orgs = await logic.LoadManyAsync<Organization, int>(ctx.Source.Organizations.Keys);
                    return orgs.Select(org => (item: org, score: ctx.Source.Organizations[org._id]));
                });

            Field<ListGraphType<ScoredSearchResultGraphType<Ticket>>>()
                .Name("tickets")
                .ResolveAsync(async ctx =>
                {
                    var tickets = await logic.LoadManyAsync<Ticket, Guid>(ctx.Source.Tickets.Keys);
                    return tickets.Select(ticket => (item: ticket, score: ctx.Source.Tickets[ticket._id]));
                });

            Field<ListGraphType<ScoredSearchResultGraphType<User>>>()
                .Name("users")
                .ResolveAsync(async ctx =>
                {
                    var users = await logic.LoadManyAsync<User, int>(ctx.Source.Users.Keys);
                    return users.Select(user => (item: user, score: ctx.Source.Users[user._id]));
                });
        }
    }
}
