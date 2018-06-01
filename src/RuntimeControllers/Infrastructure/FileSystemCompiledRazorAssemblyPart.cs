using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace RuntimeControllers
{
    public class FileSystemCompiledRazorAssemblyPart : CompiledRazorAssemblyPart
    {
        public FileSystemCompiledRazorAssemblyPart(string absolutePath, Assembly assembly) : base(assembly)
        {
            AbsolutePath = absolutePath;
        }

        public string AbsolutePath { get; }
    }
}
