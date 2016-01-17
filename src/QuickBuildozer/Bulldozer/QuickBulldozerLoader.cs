using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QuickBuildozer
{
    public class QuickBulldozerLoader : LoadingExtensionBase
    {
        private LoadMode _mode;

        private RedirectCallsState redirectState;

        public override void OnCreated(ILoading loading)
        {
            ModLogger.Debug("QuickBulldozerLoader created");
        }

        public override void OnReleased()
        {
            ModLogger.Debug("QuickBulldozerLoader released");
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            _mode = mode;
            if (mode != LoadMode.NewGame && mode != LoadMode.LoadGame) return;

            base.OnLevelLoaded(mode);
            if (mode == LoadMode.LoadGame || mode == LoadMode.NewGame)
            {
                redirectState = RedirectionHelper.RedirectCalls(
                    typeof(BulldozeTool).GetMethod("TryDeleteBuilding", BindingFlags.Instance | BindingFlags.NonPublic),
                    typeof(CustomBuldozeTool).GetMethod("TryDeleteBuilding", BindingFlags.Instance | BindingFlags.Public));
                ModLogger.Debug("Buildoze tool has been detoured");
            }
        }

        public override void OnLevelUnloading()
        {
            if (_mode != LoadMode.NewGame && _mode != LoadMode.LoadGame) return;

            base.OnLevelUnloading();
            RedirectionHelper.RevertRedirect(redirectState);
        }
    }
}
