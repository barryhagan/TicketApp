using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Linq;
using TicketBusinessLogic;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    internal class UserGraphType : ModelBaseGraphType<User, int>
    {
        public UserGraphType(IDataLoaderContextAccessor dataLoader, BusinessLogic logic) : base(dataLoader, logic)
        {
            Name = "User";

            Field(x => x.name, nullable: true);
            Field(x => x.alias, nullable: true);
            Field(x => x.active);
            Field(x => x.verified);
            Field(x => x.shared);
            Field(x => x.locale, nullable: true);
            Field(x => x.timezone, nullable: true);
            Field(x => x.last_login_at, type: typeof(DateTimeOffsetGraphType));
            Field(x => x.email, nullable: true);
            Field(x => x.phone, nullable: true);
            Field(x => x.signature, nullable: true);
            Field(x => x.organization_id);
            Field(x => x.suspended);
            Field(x => x.role, nullable: true);

            Field<OrganizationGraphType>()
                .Name("organization")
                .Resolve(ctx =>
                {
                    if (ctx.Source.organization_id > 0)
                    {
                        return DataLoadById<Organization, int>(ctx.Source.organization_id);
                    }
                    return null;
                });

            Field<ListGraphType<TicketGraphType>>()
                .Name("assignedTickets")
                .Resolve(ctx =>
                {
                    return logic.Query<Ticket, Guid>().Where(t => t.assignee_id == ctx.Source._id);
                });

            Field<ListGraphType<TicketGraphType>>()
                .Name("submittedTickets")
                .Resolve(ctx =>
                {
                    return logic.Query<Ticket, Guid>().Where(t => t.submitter_id == ctx.Source._id);
                });
        }
    }
}
