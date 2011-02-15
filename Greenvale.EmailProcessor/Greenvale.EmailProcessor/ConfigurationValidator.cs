using System;
using System.IO;

namespace Greenvale.EmailProcessor
{
    /// <summary>
    /// Takes configuration file and validates it contents to ensure 
    /// all the elements are parsable.
    /// </summary>
    public class ConfigurationValidator
    {
        /// <summary>
        /// Absolute Path to the configuration file.
        /// </summary>
        public string Path { get; set; }

        public ConfigurationValidator(string path)
        {
            // Check the file exists
            if (!File.Exists(path))
                throw new ArgumentException("The file path provided does not exist");

            this.Path = path;
        }
    }
}
