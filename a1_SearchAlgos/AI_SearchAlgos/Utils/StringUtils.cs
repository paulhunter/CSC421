using System;
using System.Collections.Generic;
using System.Text;

namespace AI_SearchAlgos.Utils
{
    class StringUtils
    {

        public static string GetDateTimeFileName()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            //TODO: Move this Format String to Congifuration
            return now.ToString("yyyyMMdd_HHmmss_ff");
        }

        public static string GetTimeStamp()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            //TODO: Move this format String to a Configuration
            return now.ToString("HH:mm:ss.fff");
        }


    }
}
