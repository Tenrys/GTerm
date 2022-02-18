﻿using System.Diagnostics;

namespace GTerm.Extensions
{
    public static class ProcessExtensions
    {
        private static string FindIndexedProcessName(int pid)
        {
            string processName = Process.GetProcessById(pid).ProcessName;
            Process[] processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (int index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                PerformanceCounter processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if ((int)processId.NextValue() == pid)
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            PerformanceCounter parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int)parentId.NextValue());
        }

        public static Process GetParent(this Process process) => FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
    }
}