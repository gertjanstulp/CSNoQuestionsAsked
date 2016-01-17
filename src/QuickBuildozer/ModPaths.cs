using ColossalFramework.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuickBuildozer
{
    public static class ModPaths
    {
        private const string ModRootFolderName = "QuickBuildozer";
        private const string ConfigurationFileName = "QuickBuildozer.xml";
        private const string LogFileName = "QuickBuildozer.log";

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
