using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class WallpaperEngine : Form
    {
        public WallpaperEngine()
        {
            if(Properties.Settings.Default.ThemeLayout == null)
            {
                foreach(Screen currentScreen in Screen.AllScreens)
                {
                    Thread wallpaperThread = new Thread(() => WallpaperInstance(currentScreen, Properties.Settings.Default.DefaultContent));
                    wallpaperThread.IsBackground = true;
                    wallpaperThread.Start();
                }
            }

            else
            {
                System.Collections.Specialized.StringCollection themeLayout = Properties.Settings.Default.ThemeLayout;

                foreach (string monitorLayout in themeLayout)
                {
                    string[] formattedLayout =  monitorLayout.Split(';');
                    Thread wallpaperThread = new Thread(() => WallpaperInstance(Screen.AllScreens[Int32.Parse(formattedLayout[0])],formattedLayout[2]));
                    wallpaperThread.IsBackground = true;
                    wallpaperThread.Start();
                }
            }
        }

        private void WallpaperInstance(Screen displayScreen, String contentAddress)
        {
            Form wallpaperInstance = new Form();

            wallpaperInstance.Name = "WallpaperInstance";
            wallpaperInstance.Text = "WallpaperInstance";
            wallpaperInstance.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            wallpaperInstance.WindowState = FormWindowState.Normal;
            wallpaperInstance.Size = displayScreen.Bounds.Size;
            wallpaperInstance.StartPosition = FormStartPosition.Manual;

            Window_API_Wrapper.DrawUnderDesktop(wallpaperInstance);

            wallpaperInstance.SuspendLayout();
            wallpaperInstance.Location = new Point(displayScreen.WorkingArea.Left - this.Location.X, displayScreen.WorkingArea.Top - this.Location.Y);

            wallpaperInstance.ResumeLayout(false);

            ChromiumWebBrowser browser = new ChromiumWebBrowser(contentAddress)
            {
                Dock = DockStyle.Fill,
            };

            wallpaperInstance.Controls.Add(browser);

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);

            wallpaperInstance.ShowDialog();
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

            public static void DrawUnderDesktop(Form wallpaperInstance)
            {
                IntPtr progman = Window_API_Wrapper.FindWindow("progman", null);

                IntPtr zero = IntPtr.Zero;
                Window_API_Wrapper.SendMessageTimeout(progman, 0x052C, zero, zero, 0x0, 1000, out zero);

                IntPtr workerw = IntPtr.Zero;

                Window_API_Wrapper.EnumWindows(new Window_API_Wrapper.EnumWindowsProc((tophandle, topparamhandle) =>
                {
                    IntPtr p = Window_API_Wrapper.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", IntPtr.Zero);
                    if (p != IntPtr.Zero) { workerw = Window_API_Wrapper.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", IntPtr.Zero); }

                    return true;
                }), IntPtr.Zero);

                Window_API_Wrapper.SetParent(wallpaperInstance.Handle, workerw);
            }
        }
    }
}
