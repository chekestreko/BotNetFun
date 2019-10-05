using System;
using System.Linq;

using SnowynxHelpers.Extensions;

namespace BotNetFun.Data
{
    internal static class Helpers
    {
        public static T GetDerivedClass<T>(string className) where T : class
        {
            className = className.RemoveWhitespace();

            Type[] listOfDerived = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from assemblyType in domainAssembly.GetTypes()
                            where assemblyType.IsSubclassOf(typeof(T)) && assemblyType.IsClass && !assemblyType.IsAbstract && typeof(T).IsAssignableFrom(assemblyType)
                            select assemblyType).ToArray();

            foreach (Type t in listOfDerived)
            {
                if (className == t.Name)
                {
                    return Activator.CreateInstance(t) as T;
                }
            }

            return null;
        }
    }
}
