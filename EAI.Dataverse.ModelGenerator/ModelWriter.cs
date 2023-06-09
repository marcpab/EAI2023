﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;

namespace EAI.Dataverse.ModelGenerator
{
    public class ModelWriter
    {
        public static string Write(Assembly modelAssembly, string code)
        {
            var autogenFilePath = GetAssemblySourceFileNames(modelAssembly).Where(fn => fn.Contains("_autogen.cs")).FirstOrDefault();
            if (string.IsNullOrEmpty(autogenFilePath))
                autogenFilePath = Path.Combine(Path.GetDirectoryName(GetAssemblySourceFileNames(modelAssembly).First()), "_autogen.cs");

            using (var writer = new StreamWriter(autogenFilePath))
                writer.Write(code);

            return autogenFilePath;
        }

        public static IEnumerable<string> GetAssemblySourceFileNames(Assembly assembly)
        {
            var streams = new List<Stream>();
            try
            {
                // Open the Portable Executable (PE) file
                using (var fs = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var peReader = new PEReader(fs))
                {
                    MetadataReaderProvider pdbReaderProvider;
                    string pdbPath;
                    if (peReader.TryOpenAssociatedPortablePdb(
                        assembly.Location,
                        fn =>
                        {
                            var stream = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            streams.Add(stream);
                            return stream;
                        },
                        out pdbReaderProvider,
                        out pdbPath))
                        using (pdbReaderProvider)
                        {
                            var pdbReader = pdbReaderProvider.GetMetadataReader();

                            foreach (var docHandle in pdbReader.Documents)
                            {
                                var doc = pdbReader.GetDocument(docHandle);

                                var docName = pdbReader.GetString(doc.Name);

                                yield return docName.Replace("\\\\", "\\");
                            }
                        }
                }
            }
            finally
            {
                foreach (var stream in streams)
                    stream.Dispose();
            }
        }
    }
}
