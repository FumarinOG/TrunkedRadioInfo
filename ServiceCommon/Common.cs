using ObjectLibrary.Interfaces;
using System;
using System.Linq;

namespace ServiceCommon
{
    public static class Common
    {
        private const string PARAMETER_SYSTEM_ID = "systemID";
        private const string PHASE_II_DELIMETER = "/";
        private static readonly string[] PHASE_II_CHANNELS = new string[] { $"{PHASE_II_DELIMETER}0", $"{PHASE_II_DELIMETER}1" };

        public static void CheckDates(IRecord record, DateTime timeStamp)
        {
            if ((timeStamp < record.FirstSeen) || (record.FirstSeen == DateTime.MinValue))
            {
                record.FirstSeen = timeStamp;
            }

            if ((timeStamp > record.LastSeen) || (record.LastSeen == DateTime.MinValue))
            {
                record.LastSeen = timeStamp;
            }
        }

        public static string FixFrequency(string frequency)
        {
            // In some circumstances Pro96Com starts appending extraneous numbers after the frequency being logged even when it's not Phase II
            if ((frequency.Contains(PHASE_II_DELIMETER)) && (!IsPhaseIIFrequency(frequency)))
            {
                return frequency.Substring(0, frequency.IndexOf(PHASE_II_DELIMETER));
            }

            return frequency;
        }

        public static bool IsPhaseIIFrequency(string frequency)
        {
            if (frequency.Contains(PHASE_II_DELIMETER))
            {
                return (PHASE_II_CHANNELS.Any(p2c => p2c.Equals(frequency.Substring(frequency.IndexOf("/")))));
            }

            return false;
        }
    }
}
