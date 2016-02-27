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

            // Assign the filename to the result. This is used later on when saving the configuration.
            ConfigurationContainer result = new ConfigurationContainer();

            // Check if the file exists. If so, deserialize it to a new instance. If not so, create a new empty instance
            if (File.Exists(filename))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        var attempt = (ConfigurationContainer)new XmlSerializer(typeof(ConfigurationContainer)).Deserialize(sr);
                        result = attempt;
                    }
                }
                catch (Exception ex)
                {
                    ModLogger.Debug("An error occured while loading the configuration file, default configuration will be applied:{0}{1}", Environment.NewLine, ex.ToString());
                }
            }
            
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

            try
            {
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
            catch (Exception ex)
            {
                ModLogger.Warning("An error occured while saving mod configuration to file '{0}'", _filename);
                ModLogger.Exception(ex);
            }
            
        }
    }
}
