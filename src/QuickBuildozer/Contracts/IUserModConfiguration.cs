using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoQuestionsAsked
{
    /// <summary>
    /// Defines methods for displaying custom configuration UI for a mod
    /// </summary>
    public interface IUserModConfiguration
    {
        /// <summary>
        /// When overriden in a derived class, creates the custom configuration UI for the mod. Called when a mod is loaded by CS. 
        /// </summary>
        /// <param name="helper">A reference to the CS helper class for creating configuration UI components</param>
        void OnSettingsUI(UIHelperBase helper);
    }
}
