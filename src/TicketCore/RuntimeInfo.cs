using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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
