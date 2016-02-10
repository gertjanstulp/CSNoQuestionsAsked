using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NoQuestionsAsked
{
    public static class ModPaths
    {
        private const string ModRootFolderName = "NoQuestionsAsked";
        private const string ConfigurationFileName = "NoQuestionsAsked.xml";
        private const string LogFileName = "NoQuestionsAsked.log";

        public static string GetConfigurationFilePath()
        {
            return Path.Combine(GetModRootPath(), ConfigurationFileName);
        }

        public static string GetLogFilePath()
        {
            return Path.Combine(GetModRootPath(), LogFileName);
        }

        private static string GetModRootPath()
        {
            return Path.Combine(DataLocation.modsPath, ModRootFolderName);
        }
    }
}
