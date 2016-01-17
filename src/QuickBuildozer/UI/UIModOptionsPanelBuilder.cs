using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBuildozer
{
    public class UIModOptionsPanelBuilder
    {
        private UIHelper _uiHelper;
        private ConfigurationContainer _configuration;
        private UIScrollablePanel _rootPanel;
        private bool _wasVisible;

        public UIModOptionsPanelBuilder(UIHelper uiHelper, ConfigurationContainer configuration)
        {
            _uiHelper = uiHelper;
            _configuration = configuration;
            _rootPanel = uiHelper.self as UIScrollablePanel;
        }

        /// <summary>
        /// This method is invoked by CS internally
        /// </summary>
        public void CreateUI()
        {
            var modSettingsGroup = _uiHelper.AddGroup(UITexts.ModSettingsGroupLabel);
            modSettingsGroup.AddCheckbox(UITexts.ModSettingsDebugLoggingOption, _configuration.DebugLogging, b => _configuration.DebugLogging = b);

            modSettingsGroup.AddCheckbox(UITexts.ModSettingsUseAltOption, _configuration.UseAlt, b => _configuration.UseAlt = b);
            modSettingsGroup.AddCheckbox(UITexts.ModSettingsUseCtrlOption, _configuration.UseCtrl, b => _configuration.UseCtrl = b);
            modSettingsGroup.AddCheckbox(UITexts.ModSettingsUseShiftOption, _configuration.UseShift, b => _configuration.UseShift = b);

            _rootPanel.eventVisibilityChanged += rootPanel_eventVisibilityChanged;
        }

        private void rootPanel_eventVisibilityChanged(UIComponent component, bool value)
        {
            // Only save and apply the configuration if the rootpanel was visible but isn't anymore (meaning the user closed the window)
            if (_wasVisible && !value)
            {
                _configuration.SaveConfiguration();
                _configuration.ApplyConfiguration();
            }
            this._wasVisible = value;
        }
    }
}
