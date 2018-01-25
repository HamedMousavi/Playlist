

namespace HLib.Language
{

    using System.Reflection;
    using System.Threading;


    public class ResourceManager
    {

        private System.Resources.ResourceManager _resMan;


        public ResourceManager()
        {
            var asm = Assembly.Load("LocalizedResources");
            _resMan = new System.Resources.ResourceManager("LocalizedResources.Resource", asm);
        }


        public string this[string resourceName]
        {
            get
            {
                return string.IsNullOrWhiteSpace(resourceName)
                           ? string.Empty
                           : _resMan.GetString(resourceName, Thread.CurrentThread.CurrentUICulture);
            }
        }
    }
}