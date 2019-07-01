using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management;
using System.Diagnostics;

namespace WebAPI.Helpers
{
    public static class ProcessManager
    {
        //source: https://stackoverflow.com/questions/9319717/how-to-kill-a-process-started-by-cmd-exe
        public static void KillProcessAndChildren(int pid)
        {
            using (var searcher = new ManagementObjectSearcher
                ("Select * From Win32_Process Where ParentProcessID=" + pid))
            {
                var moc = searcher.Get();
                foreach (ManagementObject mo in moc)
                {
                    KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
                }
                try
                {
                    var proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                catch (Exception e)
                {
                    // Process already exited.
                }
            }
        }
    }
}