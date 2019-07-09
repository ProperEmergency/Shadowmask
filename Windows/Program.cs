using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using System;
using System.Windows.Forms;

namespace Shadowmask
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            /*
             *  Setup CEF Enviornment
             */

            // For Windows 7 and above.
            Cef.EnableHighDPISupport();

            // Enable browser creation from local file path.
            var settings = new CefSettings();

            settings.RegisterScheme
            (
                new CefCustomScheme
                {
                    SchemeName = "LocalFiles",
                    DomainName = null,
                    SchemeHandlerFactory = new FolderSchemeHandlerFactory(rootFolder: Environment.CurrentDirectory)
                }
            );

            // Monitor parent process exit and close subprocesses if parent process exits first
            // This will at some point in the future becomes the default
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            // Allows for website autoplay.
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";

            // Starts browser in guest mode (a lot of this is already done by default).
            settings.CefCommandLineArgs.Add("--bwsi", "1");

            // Mutes any audio playing in the browser instances, eventually this will be configurable.
            settings.CefCommandLineArgs.Add("--mute-audio", "1");

            /*
             *  Initialize CEF Enviornment & launch Tray Client.
             */
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            Application.Run(new TrayClient());
        }
    }
}
