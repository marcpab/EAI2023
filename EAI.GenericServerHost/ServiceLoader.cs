using System.Reflection;
using System.Runtime.Loader;

namespace EAI.GenericServerHost
{
    public class ServiceLoader
    {
        private string _servicesDir;

        public string ServicesDir { get => _servicesDir; set => _servicesDir = value; }

        public Assembly? AssemblyResolver(AssemblyLoadContext context, AssemblyName assemblyName)
        {
            var currentDir = Path.GetDirectoryName(typeof(ServiceLoader).Assembly.Location);

            var assemblyDir = Path.Combine(currentDir, "services", assemblyName.Name);

            var assemblyFullPath = Path.Combine(assemblyDir, $"{assemblyName.Name}.dll");

            if (File.Exists(assemblyFullPath))
            {
                var assembly = context.LoadFromAssemblyPath(assemblyFullPath);

                foreach (var additionalAssemblyFilePath in Directory.GetFiles(assemblyDir, "*.dll"))
                    if (additionalAssemblyFilePath != assemblyFullPath)
                        try
                        {
                            context.LoadFromAssemblyPath(additionalAssemblyFilePath);
                        }
                        catch(Exception ex) 
                        { 
                        }

                return assembly;
            }

            return null;
        }
    }
}
