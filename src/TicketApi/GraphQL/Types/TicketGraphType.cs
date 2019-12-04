using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using TicketBusinessLogic;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    internal class TicketGraphType : ModelBaseGraphType<Ticket, Guid>
    {
        public TicketGraphType(IDataLoaderContextAccessor dataLoader, BusinessLogic logic) : base(dataLoader, logic)
        {
            Name = "Ticket";

            Field(x => x.type, nullable: true);
            Field(x => x.subject, nullable: true);
            Field(x => x.description, nullable: true);
            Field(x => x.priority, nullable: true);
            Field(x => x.status, nullable: true);
            Field(x => x.submitter_id);
            Field(x => x.assignee_id);
            Field(x => x.organization_id);
            Field(x => x.has_incidents);
            Field(x => x.due_at, type: typeof(DateTimeOffsetGraphType));
            Field(x => x.via, nullable: true);

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

            Field<UserGraphType>()
                .Name("assignee")
                .Resolve(ctx =>
                {
                    if (ctx.Source.assignee_id > 0)
                    {
                        return DataLoadById<User, int>(ctx.Source.assignee_id);
                    }
                    return null;
                });

            Field<UserGraphType>()
                .Name("submitter")
                .Resolve(ctx =>
                {
                    if (ctx.Source.submitter_id > 0)
                    {
                        return DataLoadById<User, int>(ctx.Source.submitter_id);
                    }
                    return null;
                });
        }
    }
}
