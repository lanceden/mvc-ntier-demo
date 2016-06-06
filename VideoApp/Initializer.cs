using System.Web;
using VideoApp;

//[assembly: PreApplicationStartMethod(typeof(Initializer), "Initialize")]

namespace VideoApp
{
    using System.Reflection;
    using System.Web.Compilation;
    public static class Initializer
    {
        public static void Initialize()
        {
            var pluginAssemblies = Commons.Helper.GetControlleraAssemblies();
            //遍歷所有的插件程序集
            foreach (var item in pluginAssemblies)
            {
                var asm = Assembly.LoadFrom(item.FullName);
                BuildManager.AddReferencedAssembly(asm);
            }
        }
    }
}
