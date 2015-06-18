using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Jexpr.Core.Impl
{
    internal class JsReferenceLoader : IScriptLoader
    {
        public string Load(Dictionary<string, string> pathList)
        {
            var scriptsCache = new StringBuilder();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var scriptPath in pathList)
            {
                using (var stream = assembly.GetManifestResourceStream(scriptPath.Value))
                {
                    if (stream != null)
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            var source = sr.ReadToEnd();
                            scriptsCache.AppendFormat("; {0}", source);
                        }
                    }
                }
            }

            return scriptsCache.ToString();
        }
    }
}