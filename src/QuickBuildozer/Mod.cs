using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBuildozer
{
    public class Mod : IUserMod, IUserModStateChange, IUserModConfiguration
    {
        private string _settingsFilePath;
        private ConfigurationContainer _configuration;

        public Mod()
        {
            ModLogger.ModLoaded();
        }

        public string Name
        {
            get { return UITexts.ModName; }
        }

        public string Description
        {
            get { return UITexts.ModDescription; }
        }

        public void OnEnabled()
        {
            _settingsFilePath = ModPaths.GetConfigurationFilePath();
            Load();
            ModLogger.Debug("QuickBuildozer mod enabled");
        }

        public void OnDisabled()
        {
            ModLogger.Debug("QuickBuildozer mod disabled");
            this.Unload();
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelper uiHelper = helper as UIHelper;
            if (uiHelper != null)
            {
                var optionsPanel = new UIModOptionsPanelBuilder(uiHelper, _configuration);
                optionsPanel.CreateUI();
                ModLogger.Debug("Options panel created");
            }
            else
                ModLogger.Warning("Could not populate the settings panel, helper is null or not a UIHelper");
        }

        private void Load()
        {
            try
            {
                _configuration = ConfigurationContainer.LoadConfiguration(_settingsFilePath);
            }
            catch (Exception ex)
            {
                ModLogger.Warning("An error occured while loading mod configuration from file '{0}', the default configuration will be applied", _settingsFilePath);
                ModLogger.Exception(ex);

                // Always create a configuration object, even when the file could not be loaded. This way the mod will not crash on configuration issues
                _configuration = new ConfigurationContainer();
            }

            // Apply the configuration to the running mod.
            _configuration.ApplyConfiguration();
        }

        private void Unload()
        {
            try
            {
                _configuration.SaveConfiguration();
            }
            catch (Exception ex)
            {
                ModLogger.Warning("An error occured while saving mod configuration to file '{0}', mod configuration is not saved", _settingsFilePath);
                ModLogger.Exception(ex);
            }
        }
    }
}
