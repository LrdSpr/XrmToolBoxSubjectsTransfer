using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace TransferCaseSubjects
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Subjects Hierarchy"),
        ExportMetadata("Description", "Load, Save and Update your subjects hierarchy"),
// Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAsQAAALEBxi1JjQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAOkSURBVFiFzZddaFtlGMd/78k5+WiTpR9rXapdN1anhoZtmQ7SZaOCiNPNTwRx4kVhUC+8c+KFQwS9EdnNZOjtEGYnExEURZl3c3MrDlw3Sqd1U6xjTdOmbXJyTs77etEuNk1PljRR/F+958n/fZ7fefK8OSeCKqXGD67D9n+knMJ+x5zJC4/+qh4/fbLa/W7SqnZa/reAF6S1ENS8oXZpZU8oVcP+ugEEu5WUOFYGJS2UQrcvPv9gvQB6tUZpZzuQCt3XihIAEiFEc70A4vbi/c9VVFPsU2J1qO3BLw8awgwvj13LJU6lC11Tq/nTWZl950XPsdvXamhoN0IkUUrMp9MLoeHhY7CsAwK+U4KIG+ml+SdWC7/m5l+wC7z+sWW/95L3w8UC4lMgghCYmYwCSgFgsXh7K4TqaKymO/z2hwcAJdm67CN/caVUsfNl7X4oBn7f2gFu5WxMG26kqvOXAWii9DqTnkY6TsUkwXALumH8k2M109NS/nhV5h0H4n0Bna9dAJZrdjrFyeNHkVJWBEg8so8d/XsreugutE39KURBgohmi+GKAOG2dobefLdy4jpV9y9ZvXLtwJnsMHlpArDZiKKN6YyOnHdNJIRgz+NPQailMQAT1mhx3SSC7Oh+GKfCMGqaxrqWNmacyvNSNcBKBZqDRLp7SmIewyAUXnHHObMxAJ16Nzk5D0CL3snoyHnOfvtVicfn8zN4+AhCW/souQIcCB4qDSRge2LPmgu56f97Ck5ljmKpPAD3eXfiGfFy7sw3FZM9N/gKWntnYwDm5ExxnZVz7O1/hujOXRWT+QNN3GrUEK7U7HSKyd+vl242DO7t21ZTwaoBYr4kllq8m3uMXlLX/2Ji7EoZQG809u+cgl2BR0sDUdgSja25UM0Ac7MzOLYNQFMohGF4yecrf7/+QFPjAI6//QYdkbsxc1k2btlKV8/m//YUAAwePsLVny4y/vMl4skB4smBOyZs2ClQUvL9F6eZujmJz+fnxrUxfrlyucTj0XWSjx1Aq2kIVXUAT758CMvM0bq+g8jGTXh0ndb1HWUAQgiXDGW62XMXGywzi+bx264ACoUE7o+X/+lp21D+1q6W9gAoBY5yeRzbgf4HuqbHHXNG0wPhAVeAlJlHWbU905dLAxay5V0R2z6YUJefTWjNYUPEPjlbBuAoJYVCO/dDHe/kS1JAwVEYXgolEH2fXVjpLQJMZuxfF/Kyt+7qS/J6hKXrnLiT72/GAjgcJyFEKQAAAABJRU5ErkJggg=="),
        ExportMetadata("BigImageBase64", "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"),

        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class SubjectsPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new TransferCaseSubjects();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public SubjectsPlugin()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}