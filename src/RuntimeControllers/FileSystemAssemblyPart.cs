using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace RuntimeControllers
{
    public class FileSystemAssemblyPart : AssemblyPart
    {
        public FileSystemAssemblyPart(string absolutePath, Assembly assembly) : base(assembly)
        {
            AbsolutePath = absolutePath;
        }

        public string AbsolutePath { get; }
    }
}
