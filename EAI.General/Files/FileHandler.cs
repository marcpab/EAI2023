using System;
using System.IO;

namespace EAI.General.Files
{
    /// <summary>
    /// Wrapper for handling file operations
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// Get the current directory
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Checks if a file exists with the given file name on the given location
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="directory"></param>
        /// <returns>true if file exists, otherwise false</returns>
        public static bool CheckFileExists(string filename, string directory)
        {
            return File.Exists(Path.Combine(directory, filename));
        }

        /// <summary>
        /// Get the directory of the assembly of a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string GetAssemblyDirectory(Type type)
        {
            return Path.GetDirectoryName(type.Assembly.Location);
        }
    }
}