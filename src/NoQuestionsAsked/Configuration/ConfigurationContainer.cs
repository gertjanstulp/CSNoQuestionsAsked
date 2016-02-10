using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NoQuestionsAsked
{
    public class ConfigurationContainer
    {
        private string _filename;

        public bool DebugLogging { get; set; }

        public bool UseAlt { get;  set; }

        public bool UseCtrl { get; set; }

        public bool UseShift { get; set; }

        public static ConfigurationContainer LoadConfiguration(string filename)
        {
            ModLogger.Debug("Loading configuration from '{0}'", filename);

            ConfigurationContainer result = null;

            // Check if the file exists. If so, deserialize it to a new instance. If not so, create a new empty instance
            if (File.Exists(filename))
            {
                using (StreamReader sr = new StreamReader(filename))
                    result = (ConfigurationContainer)new XmlSerializer(typeof(ConfigurationContainer)).Deserialize(sr);
            }
            else
                result = new ConfigurationContainer();

            // Assign the filename to the result. This is used later on when saving the configuration.
            result._filename = filename;

            ModLogger.Debug("Configuration loaded from '{0}'", filename);

            return result;
        }

        public void ApplyConfiguration()
        {
            ModLogger.Debug("Applying configuration options");
            ModLogger.DebugLogging = this.DebugLogging;
            ConfigurationAccessor.Instance.Initialize(this);
        }

        public void SaveConfiguration()
        {
            ModLogger.Debug("Saving configuration to '{0}'", _filename);

            // Make sure the directory exists
            string dirname = Path.GetDirectoryName(_filename);
            if (!string.IsNullOrEmpty(dirname) && !Directory.Exists(dirname))
                Directory.CreateDirectory(dirname);

            // Serialize the configuration to the xml file
            using (StreamWriter sw = new StreamWriter(_filename))
            {
                new XmlSerializer(this.GetType()).Serialize(sw, this);
            }
        }
    }
}
