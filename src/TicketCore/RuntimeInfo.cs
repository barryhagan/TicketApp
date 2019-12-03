using System.Reflection;

namespace TicketCore
{
    public static class RuntimeInfo
    {
        public static readonly string ApplicationVersion;
        public static readonly string ApplicationBuild;

        static RuntimeInfo()
        {
            ApplicationVersion = typeof(RuntimeInfo).GetTypeInfo().Assembly.GetName().Version.ToString();
            ApplicationBuild = typeof(RuntimeInfo).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        }
    }
}
