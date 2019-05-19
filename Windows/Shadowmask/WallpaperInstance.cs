using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowmask
{
    public partial class WallpaperInstance : Form
    {
        private readonly ChromiumWebBrowser browser;

        public WallpaperInstance(String contentAddress)
        {
            this.Name = "WallpaperInstance";
            this.Text = "WallpaperInstance";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Normal;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.StartPosition = FormStartPosition.Manual;

            Window_API_Wrapper.DrawUnderDesktop(this);


            this.SuspendLayout();
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Left - this.Location.X, Screen.PrimaryScreen.WorkingArea.Top - this.Location.Y);

            this.ResumeLayout(false);

            System.Diagnostics.Debug.Print(Screen.PrimaryScreen.Bounds.ToString());
            System.Diagnostics.Debug.Print(this.Location.ToString());
            System.Diagnostics.Debug.Print(this.Size.ToString());

            browser = new ChromiumWebBrowser(contentAddress)
            {
                Dock = DockStyle.Top
            };

            this.Controls.Add(browser);

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);


            /*var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);*/

            /*Thread thread1 = new Thread(printCurs);
            thread1.Start();*/
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

            public static void DrawUnderDesktop(WallpaperInstance wallpaperInstance)
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

        /*private void printCurs()
        {

        }*/

    }
}
