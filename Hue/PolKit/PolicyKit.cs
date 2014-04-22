using HueSaturation.API.Hue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Hue.PolKit
{
    public class PolicyKit
    {
        public static string POLICY_CURRENT_BRIDGE = "policy.currentBridge";

        private static volatile PolicyKit instance;
        private static object syncRoot = new Object();

        private PolicyKit() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public static PolicyKit Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new PolicyKit();
                    }
                }

                return instance;
            }
        }

        public void LoadFromSettings()
        {
        }
    }
}
