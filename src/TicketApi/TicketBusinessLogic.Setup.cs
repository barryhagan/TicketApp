using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TicketCore.Exceptions;
using TicketCore.Model;

namespace TicketApi
{
    public partial class TicketBusinessLogic
    {
        private const string ORG_DATA = "TicketApi.SeedData.organizations.json";
        private const string TICKET_DATA = "TicketApi.SeedData.tickets.json";
        private const string USER_DATA = "TicketApi.SeedData.users.json";

        public async Task InitializeData()
        {
            if (store.Query<User, int>().Any() || store.Query<Organization, int>().Any() || store.Query<Ticket, Guid>().Any())
            {
                logger.LogDebug("There is already data in the system of record.  Skipping data initialization.");
                return;
            }

            var orgs = LoadObjects<Organization>(ORG_DATA);
            await store.StoreAsync<Organization, int>(orgs);
            await search.AddDocuments<Organization, int>(orgs);
            logger.LogInformation($"Loaded {orgs.Count} organizations");

            var users = LoadObjects<User>(USER_DATA);
            await store.StoreAsync<User, int>(users);
            await search.AddDocuments<User, int>(users);
            logger.LogInformation($"Loaded {users.Count} users");

            var tickets = LoadObjects<Ticket>(TICKET_DATA);
            await store.StoreAsync<Ticket, Guid>(tickets);
            await search.AddDocuments<Ticket, Guid>(tickets);
            logger.LogInformation($"Loaded {tickets.Count} tickets");
        }

        private List<T> LoadObjects<T>(string resourceName)
        {
            var serializer = new JsonSerializer();
            var assembly = typeof(TicketBusinessLogic).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    return serializer.Deserialize<List<T>>(jsonReader);
                }
            }
            else
            {
                throw new TicketAppException("Unable to load embedded resource stream");
            }
        }

    }
}
