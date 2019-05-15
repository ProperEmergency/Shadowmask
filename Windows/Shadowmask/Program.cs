using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //Monitor parent process exit and close subprocesses if parent process exits first
            //This will at some point in the future becomes the default
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            //For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("enable-media-stream", "1");

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            Application.Run(new WallpaperInstance());
        }
    }
}
