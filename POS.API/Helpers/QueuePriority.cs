using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Helpers
{
    public static class QueuePriority
    {
        public const string Critical = "critical";
        public const string Default = "default";
        public const string Passive = "passive";

        //The order is important, workers will fetch jobs from the
        //critical queue first, and then from the default queue, and so on...
        public static string[] Priority = new[] { Critical, Default, Passive };
    }
}
