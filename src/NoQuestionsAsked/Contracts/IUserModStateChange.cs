using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoQuestionsAsked
{
    /// <summary>
    /// Defines methods for performing custom logic when a mod is enabled or disabled in the CS mod configuration window
    /// </summary>
    public interface IUserModStateChange
    {
        /// <summary>
        /// When overriden in a derived class, performs additional logic when:
        /// - the mod is enabled by the user in the CS mod configuration window - or -
        /// - the mod is enabled by CS at startup if the mod was already enabled previously
        /// </summary>
        /// <remarks>This method is invoked by CS internals.</remarks>
        void OnEnabled();

        /// <summary>
        /// When overriden in a derived class, performs additional logic when the mod is disabled by the user in the CS mod configuration window. 
        /// </summary>
        /// <remarks>This method is invoked by CS internals.</remarks>
        void OnDisabled();
    }
}
