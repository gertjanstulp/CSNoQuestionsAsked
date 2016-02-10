using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoQuestionsAsked
{
    public static class UITexts
    {
        public static string ModName
        {
            get { return "No Questions Asked"; }
        }

        public static string ModDescription
        {
            get { return "Changes the default bulldozer behavior on assets that require user confirmation when bulldozing"; }
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
            get { return "Use 'Alt' key for disabling confirmation messages"; }
        }

        public static string ModSettingsUseCtrlOption
        {
            get { return "Use 'Ctrl' key for disabling confirmation messages"; }
        }

        public static string ModSettingsUseShiftOption
        {
            get { return "Use 'Shift' key for disabling confirmation messages"; }
        }
    }
}
