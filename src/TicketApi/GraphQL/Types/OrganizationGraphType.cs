using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Linq;
using TicketBusinessLogic;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    internal class OrganizationGraphType : ModelBaseGraphType<Organization, int>
    {
        public OrganizationGraphType(IDataLoaderContextAccessor dataLoader, BusinessLogic logic) : base(dataLoader, logic)
        {
            Name = "Organization";

            Field(x => x.name, nullable: true);
            Field(x => x.domain_names, type: typeof(ListGraphType<StringGraphType>));
            Field(x => x.details, nullable: true);
            Field(x => x.shared_tickets);

            Field<ListGraphType<UserGraphType>>()
                .Name("users")
                .Resolve(ctx =>
                {
                    return logic.Query<User, int>().Where(u => u.organization_id == ctx.Source._id);
                });

            Field<ListGraphType<TicketGraphType>>()
                .Name("tickets")
                .Resolve(ctx =>
                {
                    return logic.Query<Ticket, Guid>().Where(t => t.organization_id == ctx.Source._id);
                });
        }
    }
}
