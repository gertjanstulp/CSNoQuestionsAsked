using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoQuestionsAsked
{
    public class ConfigurationAccessor
    {
        private static readonly ConfigurationAccessor _instance = new ConfigurationAccessor();

        public static ConfigurationAccessor Instance
        {
            get { return _instance; }
        }

        public bool UseAlt { get; private set; }

        public bool UseCtrl { get; private set; }

        public bool UseShift { get; private set; }

        public void Initialize(ConfigurationContainer configurationContainer)
        {
            UseAlt = configurationContainer.UseAlt;
            UseCtrl = configurationContainer.UseCtrl;
            UseShift = configurationContainer.UseShift;
        }
    }
}
