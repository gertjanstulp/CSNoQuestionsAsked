using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickBuildozer
{
    public static class UITexts
    {
        public static string ModName
        {
            get { return "QuickBuildozer"; }
        }

        public static string ModDescription
        {
            get { return "Changes the default bulldozer behavior on asking confirmation before deletion"; }
        }

        public static string ModSettingsGroupLabel
        {
            get { return "Mod settings"; }
        }

        public static string ModSettingsDebugLoggingOption
        {
            get { return "Enable debug logging (for advanced usage only)"; }
        }

        public static string ModSettingsUseAltOption
        {
            get { return "Use 'Alt' key for activating QuickBulldozer"; }
        }

        public static string ModSettingsUseCtrlOption
        {
            get { return "Use 'Ctrl' key for activating QuickBulldozer"; }
        }

        public static string ModSettingsUseShiftOption
        {
            get { return "Use 'Shift' key for activating QuickBulldozer"; }
        }
    }
}
