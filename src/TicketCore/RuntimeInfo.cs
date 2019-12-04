using System.Reflection;

namespace TicketCore
{
    public static class RuntimeInfo
    {
        public static readonly string ApplicationVersion = typeof(RuntimeInfo).GetTypeInfo().Assembly.GetName().Version.ToString();
        public static readonly string ApplicationBuild = typeof(RuntimeInfo).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}
