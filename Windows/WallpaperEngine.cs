using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class WallpaperEngine : Form
    {
        private static List<Form> wallpaperInstances = new List<Form>();

        public WallpaperEngine()
        {
            Initalize_Engine();
        }

        public void Change_Displayed_Content()
        {
            int monitorNumber = Screen.AllScreens.Count() - 1;

            foreach (Form dynamicWallpaper in wallpaperInstances)
            {
                ChromiumWebBrowser browser = dynamicWallpaper.Controls.OfType<ChromiumWebBrowser>().FirstOrDefault();
                try
                {
                    System.Diagnostics.Debug.WriteLine(Properties.Settings.Default.ThemeLayout[monitorNumber].Split(';')[2]);
                    browser.Load(Properties.Settings.Default.ThemeLayout[monitorNumber].Split(';')[2]);
                }
                catch (NullReferenceException)
                {
                    browser.Load(Properties.Settings.Default.DefaultContent);
                }
                monitorNumber--;
            }
        }

        private void Initalize_Engine()
        {
            List<Form> wallpaperInstances = new List<Form>();

            if (Properties.Settings.Default.ThemeLayout == null)
            {
                foreach (Screen currentScreen in Screen.AllScreens)
                {
                    var wallpaperThread = Task.Run(() => WallpaperInstance(currentScreen, Properties.Settings.Default.DefaultContent));
                }
            }

            else
            {
                System.Collections.Specialized.StringCollection themeLayout = Properties.Settings.Default.ThemeLayout;

                foreach (string monitorLayout in themeLayout)
                {
                    string[] formattedLayout = monitorLayout.Split(';');
                    var wallpaperThread = Task.Run(() => WallpaperInstance( Screen.AllScreens[Int32.Parse(formattedLayout[0]) - 1], formattedLayout[2]));
                }
            }
        }

        private Form WallpaperInstance(Screen displayScreen, String content)
        {
            Form wallpaperInstance = new Form();
            wallpaperInstances.Add(wallpaperInstance);

            wallpaperInstance.Name = "WallpaperInstance";
            wallpaperInstance.Text = "WallpaperInstance";
            wallpaperInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            wallpaperInstance.WindowState = FormWindowState.Normal;
            wallpaperInstance.Size = displayScreen.Bounds.Size;
            wallpaperInstance.StartPosition = FormStartPosition.Manual;
            wallpaperInstance.Location = new Point(displayScreen.WorkingArea.Left - this.Location.X, displayScreen.WorkingArea.Top - this.Location.Y);

            ChromiumWebBrowser browser = new ChromiumWebBrowser(content, null)
            {
                Dock = DockStyle.Fill,
            };

            wallpaperInstance.Controls.Add(browser);

            Window_API_Wrapper.DrawUnderDesktop(wallpaperInstance);

            wallpaperInstance.ShowDialog();

            return wallpaperInstance;
        }

        private class Window_API_Wrapper
        {
            delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, uint flags, uint timeout, out IntPtr result);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

            /*
             * This method finds (or creates if necessery) a window between the desktop icons and the wallpaper.
             * It then moves the input form under said window.
             * 
             * Based off of the concepts present in Gerald Degeneve's "Draw Behind Desktop Icons in Windows 8+", linked here:
             * https://www.codeproject.com/Articles/856020/Draw-behind-Desktop-Icons-in-Windows
             * 
             */
            public static void DrawUnderDesktop(Form wallpaperInstance)
            {
                // Find the window handle for the shell.
                IntPtr progman = Window_API_Wrapper.FindWindow("progman", null);

                // Direct the shell to spawn a worker window behind the desktop icons, this may or may not already be present in Win 8/10.
                IntPtr zero = IntPtr.Zero;
                Window_API_Wrapper.SendMessageTimeout(progman, 0x052C, zero, zero, 0x0, 1000, out zero);

                // Find the handle of the new worker window.
                IntPtr workerw = IntPtr.Zero;
                Window_API_Wrapper.EnumWindows(new Window_API_Wrapper.EnumWindowsProc((tophandle, topparamhandle) =>
                {
                    IntPtr p = Window_API_Wrapper.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);
                    if (p != IntPtr.Zero) { workerw = Window_API_Wrapper.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", IntPtr.Zero); }

                    return true;
                }), IntPtr.Zero);

                // Set that window as the parent of our form, drawing under the desktop!
                Window_API_Wrapper.SetParent(wallpaperInstance.Handle, workerw);
            }
        }
    }
}
