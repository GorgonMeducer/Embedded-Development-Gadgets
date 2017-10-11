using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Markup;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Build.BuildEngine;
using System.ComponentModel;

namespace ESnail.Utilities.XAML
{
    



    public static class XAMLHelper
    {

        public static void Save(object obj, Stream stream)
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(path);

            try
            {
                string xamlFile = Path.Combine(path, "input.xaml");
                string projFile = Path.Combine(path, "project.proj");

                using (FileStream fs = File.Create(xamlFile))
                {
                    XamlWriter.Save(obj, fs);
                }

                Engine engine = new Engine();
#pragma warning disable CS0618 // Type or member is obsolete
                engine.BinPath = RuntimeEnvironment.GetRuntimeDirectory();
#pragma warning restore CS0618 // Type or member is obsolete
                Project project = engine.CreateNewProject();
                BuildPropertyGroup pgroup = project.AddNewPropertyGroup(false);
                pgroup.AddNewProperty("AssemblyName", "temp");
                pgroup.AddNewProperty("OutputType", "Library");
                pgroup.AddNewProperty("IntermediateOutputPath", ".");
                pgroup.AddNewProperty("MarkupCompilePass1DependsOn", "ResolveReferences");

                BuildItemGroup igroup = project.AddNewItemGroup();
                igroup.AddNewItem("Page", "input.xaml");
                igroup.AddNewItem("Reference", "WindowsBase");
                igroup.AddNewItem("Reference", "PresentationCore");
                igroup.AddNewItem("Reference", "PresentationFramework");

                project.AddNewImport(@"$(MSBuildBinPath)\Microsoft.CSharp.targets", null);
                project.AddNewImport(@"$(MSBuildBinPath)\Microsoft.WinFX.targets", null);
                project.FullFileName = projFile;

                if (engine.BuildProject(project, "MarkupCompilePass1"))
                {
                    byte[] buffer = new byte[1024];
                    using (FileStream fs = File.OpenRead(Path.Combine(path, "input.baml")))
                    {
                        int read = 0;
                        while (0 < (read = fs.Read(buffer, 0, buffer.Length)))
                        {
                            stream.Write(buffer, 0, read);
                        }
                    }
                }
                else
                {
                    // attach a logger to the Engine if you need better errors
                    throw new System.Exception("Baml compilation failed.");
                }
            }
            finally
            {
                Directory.Delete(path, true);
            }
        }

        public static object Load(Stream stream)
        {
            ParserContext pc = new ParserContext();
            return typeof(XamlReader)
                .GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static)
                .Invoke(null, new object[] { stream, pc, null, false });
        }
    }



}
