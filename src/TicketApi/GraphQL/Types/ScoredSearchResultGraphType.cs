using GraphQL.Types;
using System;
using TicketCore.Model;

namespace TicketApi.GraphQL.Types
{
    internal class ScoredSearchResultGraphType<T> : ObjectGraphType<(T item, float score)>
    {
        public ScoredSearchResultGraphType()
        {
            Name = $"ScoredSearchResult<{typeof(T).Name}>";

            Type itemType = null;

            switch (typeof(T).Name)
            {
                case nameof(User):
                    itemType = typeof(UserGraphType);
                    break;
                case nameof(Organization):
                    itemType = typeof(OrganizationGraphType);
                    break;
                case nameof(Ticket):
                    itemType = typeof(TicketGraphType);
                    break;
            }

            Field(itemType, "item", resolve: ctx => ctx.Source.item);

            Field<FloatGraphType>()
                .Name("score")
                .Resolve(ctx => ctx.Source.score);
        }
    }
}
