using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TicketCore.Exceptions;
using TicketCore.Interfaces;
using TicketCore.Model;

namespace TicketBusinessLogic
{
    public class EmbeddedJsonDataLoader : IDataLoader
    {
        private const string ORG_DATA = "TicketBusinessLogic.SeedData.organizations.json";
        private const string TICKET_DATA = "TicketBusinessLogic.SeedData.tickets.json";
        private const string USER_DATA = "TicketBusinessLogic.SeedData.users.json";

        public IEnumerable<T> LoadObjects<T>()
        {
            var serializer = new JsonSerializer();
            var assembly = typeof(EmbeddedJsonDataLoader).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(GetEmbeddedResourceName<T>());
            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<List<T>>(jsonReader).AsEnumerable();
                }
            }
            else
            {
                throw new TicketAppException("Unable to load embedded resource stream");
            }
        }

        private string GetEmbeddedResourceName<T>()
        {
            switch (typeof(T).Name)
            {
                case nameof(Organization):
                    return ORG_DATA;
                case nameof(Ticket):
                    return TICKET_DATA;
                case nameof(User):
                    return USER_DATA;
                default:
                    throw new TicketAppException($"There is no source data available for objects of type ${typeof(T).Name}");
            }
        }

    }
}
